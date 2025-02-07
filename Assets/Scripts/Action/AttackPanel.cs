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

            equipmentUnit.Setup(equipment);
        }
    }
}
