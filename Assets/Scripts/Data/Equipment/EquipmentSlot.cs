using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : Unit
{
    public Equipment equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image plusIcon;
    [SerializeField] Image blockIcon;
    [SerializeField] Image cursor;
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] EquipmentTypeList equipmentTypeList;
    [SerializeField] EquipmentDialog equipmentDialog;
    public bool isActive = false;

//SetUpより前に実行したい
    private void Awake()
    {
        Debug.Log("EquipmentSlot Start");
        plusIcon.gameObject.SetActive(true);
        blockIcon.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        if(equipmentType != null)
        {
            SetEquipmentType();
        }
    }

    public void Setup(Equipment equipment)
    {
        this.equipment = equipment;
        image.sprite = equipment.Base.Sprite;
        equipmentDialog.gameObject.SetActive(true);
        equipmentDialog.Setup(equipment);
        Debug.Log("EquipmentSlot Setup");
        plusIcon.gameObject.SetActive(false);
        blockIcon.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
    }

    public void OnPointerEnter()
    {
        if (equipment != null)
        {
            equipmentDialog.ShowDialog(true);
            StartCoroutine(OnPointer(true));
        }
    }

    public void OnPointerExit()
    {
        if (equipment != null)
        {
            equipmentDialog.ShowDialog(false);
            StartCoroutine(OnPointer(true));
        }
    }

    private void SetEquipmentType()
    {
        plusIcon.sprite = equipmentTypeList.GetIcon(equipmentType);
    }

    public void SetTarget(bool activeFlg)
    {
        Debug.Log("EquipmentSlot SetTarget");
        if (isActive == activeFlg) return;
        isActive = activeFlg;

        // 背景の透明度を変更する。
        Color bgColor = cursor.color;
        bgColor.a = isActive ? 1f : 0f;
        cursor.color = bgColor;
    }
}
