using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : Unit
{
    [SerializeField] Image plusIcon;
    [SerializeField] Image blockIcon;
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] EquipmentTypeList equipmentTypeList;
    [SerializeField] bool isBlock = false;

    //Setupより前に実行したい
    private void Awake()
    {
        Debug.Log("EquipmentSlot Start");
        plusIcon.gameObject.SetActive(true);
        blockIcon.gameObject.SetActive(false);
        if (isBlock)
        {
            plusIcon.gameObject.SetActive(false);
            blockIcon.gameObject.SetActive(true);
        }
        else if (equipmentType != null)
        {
            SetEquipmentType();
        }
    }

    private void SetEquipmentType()
    {
        plusIcon.sprite = equipmentTypeList.GetIcon(equipmentType);
    }
}
