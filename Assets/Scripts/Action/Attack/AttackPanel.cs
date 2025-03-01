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

    private int lifeCost = 0;
    private int batteryCost = 0;
    private int soulCost = 0;

    List<EquipmentUnit> equipmentUnitList = new List<EquipmentUnit>();

    private void Init()
    {
        RefreshEnegyCost();
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetEquipmentList();
            SetEnegyCost();
        }
    }

    private void OnEnable()
    {
        RefreshEnegyCost();

        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetEquipmentList();
            SetEnegyCost();
        }
        else
        {
            // Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    private void SetEquipmentList()
    {
        equipmentUnitList.Clear();

        foreach (Transform child in equipmentList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var equipment in playerUnit.Battler.Equipments)
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

    public List<Damage> ActivateEquipments()
    {
        Debug.Log("equipmentUnitList.Count:" + equipmentUnitList.Count);
        List<Damage> damages = new List<Damage>();

        foreach (EquipmentUnit equipment in equipmentUnitList)
        {
            if (CheckEnegy(equipment) == false)
            {
                continue;
            }
            if (Random.Range(0, 100) < equipment.Equipment.Base.Probability)
            {
                UseEnegy(equipment);
                foreach (var attack in equipment.Equipment.Base.AttackList)
                {
                    Damage damage = new Damage(AttackType.Enegy, (int)attack.type, attack.val);
                    damages.Add(damage);
                }
                equipment.SetEquipmentMotion(EquipmentUnitMotionType.Jump);
            }
        }

        Debug.Log($"ActivateEquipments:skills:{damages.Count}");

        return damages;
    }

    public bool CheckEnegy(EquipmentUnit equipment)
    {
        if (
            equipment.Equipment.Base.LifeCost.val <= playerUnit.Battler.Life &&
            equipment.Equipment.Base.BatteryCost.val <= playerUnit.Battler.Battery &&
            equipment.Equipment.Base.SoulCost.val <= playerUnit.Battler.Soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(EquipmentUnit equipment)
    {
        playerUnit.Battler.Life -= equipment.Equipment.Base.LifeCost.val;
        playerUnit.Battler.Battery -= equipment.Equipment.Base.BatteryCost.val;
        playerUnit.Battler.Soul -= equipment.Equipment.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
