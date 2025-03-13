using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentWindow : MonoBehaviour
{
    [SerializeField] EquipmentSlot head;
    [SerializeField] EquipmentSlot body;
    [SerializeField] EquipmentSlot arm1;
    [SerializeField] EquipmentSlot arm2;
    [SerializeField] EquipmentSlot leg;
    [SerializeField] GameObject accessoryList;
    [SerializeField] EquipmentCard equipmentPrefab;  // ItemUnitのプレハブ

    public Battler playerBattler;

    public void SetUp(Battler battler)
    {
        playerBattler = battler;
        SetEquipment(playerBattler.Equipments);
    }

    public void SetEquipment(List<Equipment> equipments)
    {
        foreach (Equipment equipment in equipments)
        {
            if (equipment.Base.Type == EquipmentType.Head)
                Debug.Log("Head");
            if (equipment.Base.Type == EquipmentType.Body)
                Debug.Log("Body");
            if (equipment.Base.Type == EquipmentType.Arm)
                Debug.Log("Arm");
            if (equipment.Base.Type == EquipmentType.Leg)
                Debug.Log("Leg");
            if (equipment.Base.Type == EquipmentType.Accessory)
                Debug.Log("Accessory");
        }
    }
}
