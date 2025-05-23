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

    private int headWidth = 150;
    private int equipmentWidth = 125;
    private int panelHeight = 150;

    public void Start()
    {
        playerBattler = playerUnit.Battler;
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
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

    public void SetPanelSize()
    {
        int row = playerBattler.EquipmentList.Count;
        int width = equipmentWidth * row + headWidth;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, panelHeight);
    }

    private void SetEquipmentList()
    {
        equipmentUnitList.Clear();

        foreach (Transform child in equipmentList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var equipment in playerBattler.EquipmentList)
        {
            // EquipmentUnitのインスタンスを生成
            GameObject equipmentUnitObject = Instantiate(equipmentUnitPrefab, equipmentList.transform);
            equipmentUnitObject.gameObject.SetActive(true);
            EquipmentUnit equipmentUnit = equipmentUnitObject.GetComponent<EquipmentUnit>();
            equipmentUnitList.Add(equipmentUnit);
            equipmentUnit.Setup(equipment);
        }
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
                lifeCost += equipmentUnit.Equipment.EquipmentBase.LifeCost.val;
                batteryCost += equipmentUnit.Equipment.EquipmentBase.BatteryCost.val;
                soulCost += equipmentUnit.Equipment.EquipmentBase.SoulCost.val;
            }
            else
            {
                equipmentUnit.SetStatus(UnitStatus.EnegyOut);
            }
        }

        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    public void ExecuteAttack()
    {
        if (executeFlg)
        {
            List<Attack> attacks = new List<Attack>();
            foreach (EquipmentUnit equipmentUnit in equipmentUnitList)
            {
                if (!CheckEnegy(equipmentUnit.Equipment))
                {
                    equipmentUnit.SetStatus(UnitStatus.EnegyOut);
                    continue;
                }

                if (Random.Range(0, 100) < equipmentUnit.Equipment.EquipmentBase.Probability)
                {
                    UseEnegy(equipmentUnit.Equipment);
                    attacks.Add(equipmentUnit.Equipment.Attack);
                }
            }
            attackSystem.ExecutePlayerAttack(attacks);
            CountEnegyCost();
        }
    }

    public bool CheckEnegy(Equipment equipment)
    {
        int life = Mathf.Max(0, playerBattler.Life);
        int battery = Mathf.Max(0, playerBattler.Battery);
        int soul = Mathf.Max(0, playerBattler.Soul);

        return
            equipment.EquipmentBase.LifeCost.val <= life &&
            equipment.EquipmentBase.BatteryCost.val <= battery &&
            equipment.EquipmentBase.SoulCost.val <= soul;
    }

    public void UseEnegy(Equipment equipment)
    {
        playerBattler.Life -= equipment.EquipmentBase.LifeCost.val;
        playerBattler.Battery -= equipment.EquipmentBase.BatteryCost.val;
        playerBattler.Soul -= equipment.EquipmentBase.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
