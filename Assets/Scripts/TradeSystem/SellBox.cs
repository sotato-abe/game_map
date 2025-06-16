using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// ドロップされたItem,Commandを削除するためのTrashWindowクラス
public class SellBox : TrashBox
{
    [SerializeField] PlayerUnit playerUnit;

    public override void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();
        if (droppedItemBlock)
        {
            droppedItemBlock.SellItem();
        }else if (droppedCommandBlock)
        {
            droppedCommandBlock.SellCommand();

        }
    }
}
