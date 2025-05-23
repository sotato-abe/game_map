using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// バックで使用するアイテムのクラス
// 装備、消耗品、トレジャーをすべて受け入れてバックに表示するためのクラス
public class ItemBlock : Block
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image cursor;
    [SerializeField] ConsumableDialog consumableDialog;
    [SerializeField] EquipmentDialog equipmentDialog;
    [SerializeField] TreasureDialog treasureDialog;
    private bool isActive = false;

    public void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;

        if (item is Consumable)
        {
            consumableDialog.Setup(item);
        }
        else if (item is Equipment)
        {
            equipmentDialog.Setup(item);
        }
        else if (item is Treasure)
        {
            treasureDialog.Setup(item);
        }
    }

    public void OnPointerEnter()
    {
        ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        ShowDialog(false);
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

    private void ShowDialog(bool showFlg)
    {
        if (Item is Consumable)
        {
            consumableDialog.ShowDialog(showFlg);
        }
        else if (Item is Equipment)
        {
            equipmentDialog.ShowDialog(showFlg);
        }
        else if (Item is Treasure)
        {
            treasureDialog.ShowDialog(showFlg);
        }
    }
}
