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

    private void Awake()
    {
        equipmentDialog.gameObject.SetActive(false);
    }

    public void Setup(Equipment equipment)
    {
        this.equipment = equipment;
        image.sprite = equipment.Base.Sprite;
        equipmentDialog.gameObject.SetActive(true);
        equipmentDialog.Setup(equipment);
    }

    public void OnPointerEnter()
    {
        equipmentDialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        equipmentDialog.ShowDialog(false);
        StartCoroutine(OnPointer(false));
    }

    public void SetTarget(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;

        // 背景の透明度を変更する。
        Color bgColor = cursor.color;
        bgColor.a = isActive ? 1f : 0f;
        cursor.color = bgColor;
    }
}
