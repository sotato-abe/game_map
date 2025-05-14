using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemBlock : Block
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    [SerializeField] ItemDialog itemDialog;
    private bool isActive = false;

    public void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
        itemDialog.gameObject.SetActive(true);
        itemDialog.Setup(Item);
    }

    public void OnPointerEnter()
    {
        itemDialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        itemDialog.ShowDialog(false);
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
