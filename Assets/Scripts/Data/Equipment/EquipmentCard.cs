using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCard : Unit
{
    public Equipment equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    [SerializeField] EquipmentDialog equipmentDialog;
    public bool isActive = false;

//SetUpより前に実行したい
    private void Awake()
    {
        equipmentDialog.gameObject.SetActive(false);
    }

    public void Setup(Equipment equipment)
    {
        this.equipment = equipment;
        image.sprite = equipment.Base.Sprite;
        equipmentDialog.Setup(equipment);
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
