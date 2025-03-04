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
    [SerializeField] EquipmentSlot equipmentPrefab;  // ItemUnitのプレハブ

    public void SetEquipment(List<Equipment> equipments)
    {
        foreach (Equipment equipment in equipments)
        {
            if (equipment.Base.Type == EquipmentType.Head)
                head.Setup(equipment);
            if (equipment.Base.Type == EquipmentType.Body)
                body.Setup(equipment);
            if (equipment.Base.Type == EquipmentType.Arm)
                if (arm1.equipment == null)
                {
                    arm1.Setup(equipment);
                }
                else if (arm2.equipment == null)
                {
                    arm2.Setup(equipment);
                }
            // arm2のことも考える
            if (equipment.Base.Type == EquipmentType.Leg)
                leg.Setup(equipment);
            if (equipment.Base.Type == EquipmentType.Accessory)
                Debug.Log("Accessory");
        }
    }
}
