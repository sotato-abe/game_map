using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPanel : Panel
{
    [SerializeField] GameObject equipmentUnitPrefab;  // EquipmentUnitのプレハブ
    [SerializeField] GameObject equipmentList;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] AttackSystem attackSystem;

    private Battler playerBattler;

    List<EquipmentUnit> equipmentUnitList = new List<EquipmentUnit>();

    public void Start()
    {
        playerBattler = playerUnit.Battler;
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetEquipmentList();
        CountEnegyCost();
    }

    public void Update()
    {
        if (executeFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteAttack();
            }
        }
    }

    private void SetEquipmentList()
    {
        equipmentUnitList.Clear();

        foreach (Transform child in equipmentList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var equipment in playerBattler.Equipments)
        {
            // EquipmentUnitのインスタンスを生成
            GameObject equipmentUnitObject = Instantiate(equipmentUnitPrefab, equipmentList.transform);
            equipmentUnitObject.gameObject.SetActive(true);
            EquipmentUnit equipmentUnit = equipmentUnitObject.GetComponent<EquipmentUnit>();
            equipmentUnitList.Add(equipmentUnit);
            equipmentUnit.Setup(equipment);
        }
    }

    public void ExecuteAttack()
    {
        if (executeFlg)
        {
            List<Damage> damages = ActivateEquipments();
            attackSystem.ExecutePlayerAttack(damages);
            CountEnegyCost();
        }
    }

    public List<Damage> ActivateEquipments()
    {
        List<Damage> damages = new List<Damage>();

        foreach (EquipmentUnit equipmentUnit in equipmentUnitList)
        {
            if (CheckEnegy(equipmentUnit.Equipment) == false)
            {
                continue;
            }
            if (Random.Range(0, 100) < equipmentUnit.Equipment.Base.Probability)
            {
                UseEnegy(equipmentUnit.Equipment);
                foreach (var attack in equipmentUnit.Equipment.Base.AttackList)
                {
                    Damage damage = new Damage(AttackType.Enegy, attack.type, attack.val);
                    // TODO : ダメージにステータスによる増減を追加（計算は仮で構築中）
                    if (attack.type == EnegyType.Life)
                    {
                        damage.Val = Mathf.FloorToInt(damage.Val + playerBattler.Attack.val);
                    }
                    else if (attack.type == EnegyType.Battery)
                    {
                        Debug.Log("Battery Damage : 考え中");
                    }
                    else if (attack.type == EnegyType.Soul)
                    {
                        Debug.Log("Soul Damage : 考え中");
                    }
                    damages.Add(damage);
                }
                equipmentUnit.SetEquipmentMotion(EquipmentUnitMotionType.Jump);
            }
        }

        return damages;
    }

    public bool CheckEnegy(Equipment equipment)
    {
        int life = playerBattler.Life < 0 ? 0 : playerBattler.Life;
        int battery = playerBattler.Battery < 0 ? 0 : playerBattler.Battery;
        int soul = playerBattler.Soul < 0 ? 0 : playerBattler.Soul;

        if (
            equipment.Base.LifeCost.val <= life &&
            equipment.Base.BatteryCost.val <= battery &&
            equipment.Base.SoulCost.val <= soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(Equipment equipment)
    {
        playerBattler.Life -= equipment.Base.LifeCost.val;
        playerBattler.Battery -= equipment.Base.BatteryCost.val;
        playerBattler.Soul -= equipment.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
    
    public void CountEnegyCost()
    {
        int lifeCost = 0;
        int batteryCost = 0;
        int soulCost = 0;

        foreach (EquipmentUnit equipmentUnit in equipmentUnitList)
        {
            if (CheckEnegy(equipmentUnit.Equipment))
            {
                lifeCost += equipmentUnit.Equipment.Base.LifeCost.val;
                batteryCost += equipmentUnit.Equipment.Base.BatteryCost.val;
                soulCost += equipmentUnit.Equipment.Base.SoulCost.val;
            }
        }

        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }
}
