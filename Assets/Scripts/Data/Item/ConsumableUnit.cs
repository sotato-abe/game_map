using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConsumableUnit : Unit
{
    public Consumable Consumable { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    // [SerializeField] ConsumableDialog dialog;
    private bool isActive = false;

    public virtual void Setup(Consumable consumable)
    {
        Consumable = consumable;
        image.sprite = Consumable.Base.Sprite;
        // dialog.Setup(Consumable);
    }

    public void OnPointerEnter()
    {
        // dialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        // dialog.ShowDialog(false);
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
