using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPanel : MonoBehaviour
{
    [SerializeField] GameObject equipmentUnitPrefab;  // EquipmentUnitのプレハブ
    [SerializeField] GameObject equipmentList;
    [SerializeField] BattleUnit playerUnit;

    List<EquipmentUnit> equipmentUnitList = new List<EquipmentUnit>();

    private void Init()
    {
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

    public List<Skill> ActivateEquipments()
    {
        List<Skill> skills = new List<Skill>();


        foreach (EquipmentUnit equipment in equipmentUnitList)
        {
            if (Random.Range(0, 100) < equipment.Equipment.Base.Probability)
            {
                equipment.SetEquipmentMotion(EquipmentUnitMotionType.Jump);
                skills.AddRange(equipment.Equipment.Base.SkillList);
            }
        }

        Debug.Log($"ActivateEquipments:skills:{skills.Count}");

        return skills;
    }
}
