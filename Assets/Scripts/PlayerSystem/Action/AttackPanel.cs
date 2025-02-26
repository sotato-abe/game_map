using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPanel : Panel
{
    [SerializeField] GameObject equipmentUnitPrefab;  // EquipmentUnitのプレハブ
    [SerializeField] GameObject equipmentList;
    [SerializeField] BattleUnit playerUnit;

    List<EquipmentUnit> equipmentUnitList = new List<EquipmentUnit>();

    private void Init()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetEquipmentDialog();
        }
        else
        {
            // Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetEquipmentDialog();
        }
        else
        {
            // Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    private void SetEquipmentDialog()
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
        }
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
        playerUnit.UpdateUI();
    }
}
