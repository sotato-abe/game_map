using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// ドロップされたItem,Commandを削除するためのTrashWindowクラス
public class TrashWindow : Unit, IDropHandler
{
    void Start()
    {
        scale = 1.4f;
    }
    public void OnPointerEnter()
    {
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        StartCoroutine(OnPointer(false));
    }

    public void OnDrop(PointerEventData eventData)
    {
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (droppedCommandBlock)
        {
            droppedCommandBlock.RemoveCommand();
        }
        else if (droppedItemBlock)
        {
            droppedItemBlock.RemoveItem();
        }
    }
}
