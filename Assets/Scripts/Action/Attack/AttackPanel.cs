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

    private int lifeCost = 0;
    private int batteryCost = 0;
    private int soulCost = 0;

    List<EquipmentUnit> equipmentUnitList = new List<EquipmentUnit>();

    public void Start()
    {
        playerBattler = playerUnit.Battler;
    }

    private void OnEnable()
    {
        RefreshEnegyCost();
        if (playerUnit != null && playerBattler != null)
        {
            playerBattler = playerUnit.Battler;
            SetEquipmentList();
            SetEnegyCost();
        }
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
            CountEnegyCost(equipment);
        }
    }

    public void CountEnegyCost(Equipment equipment)
    {
        foreach (Enegy cost in equipment.Base.CostList)
        {
            switch (cost.type)
            {
                case EnegyType.Life:
                    lifeCost += cost.val;
                    break;
                case EnegyType.Battery:
                    batteryCost += cost.val;
                    break;
                case EnegyType.Soul:
                    soulCost += cost.val;
                    break;
            }
        }
    }

    public void SetEnegyCost()
    {
        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    public void RefreshEnegyCost()
    {
        lifeCost = 0;
        batteryCost = 0;
        soulCost = 0;
        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    private void ExecuteAttack()
    {
        List<Damage> damages = ActivateEquipments();
        attackSystem.ExecutePlayerAttack(damages);
    }

    public List<Damage> ActivateEquipments()
    {
        List<Damage> damages = new List<Damage>();

        foreach (EquipmentUnit equipmentUnit in equipmentUnitList)
        {
            if (CheckEnegy(equipmentUnit) == false)
            {
                continue;
            }
            if (Random.Range(0, 100) < equipmentUnit.equipment.Base.Probability)
            {
                UseEnegy(equipmentUnit);
                foreach (var attack in equipmentUnit.equipment.Base.AttackList)
                {
                    Damage damage = new Damage(AttackType.Enegy, (int)attack.type, attack.val);
                    damages.Add(damage);
                }
                equipmentUnit.SetEquipmentMotion(EquipmentUnitMotionType.Jump);
            }
        }

        return damages;
    }

    public bool CheckEnegy(EquipmentUnit equipmentUnit)
    {
        if (
            equipmentUnit.equipment.Base.LifeCost.val <= playerBattler.Life &&
            equipmentUnit.equipment.Base.BatteryCost.val <= playerBattler.Battery &&
            equipmentUnit.equipment.Base.SoulCost.val <= playerBattler.Soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(EquipmentUnit equipmentUnit)
    {
        playerBattler.Life -= equipmentUnit.equipment.Base.LifeCost.val;
        playerBattler.Battery -= equipmentUnit.equipment.Base.BatteryCost.val;
        playerBattler.Soul -= equipmentUnit.equipment.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
