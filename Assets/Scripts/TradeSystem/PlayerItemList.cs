using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerItemList : MonoBehaviour, IDropHandler
{
    [SerializeField] int maxRow = 10;
    [SerializeField] public GameObject itemList;
    int defaultItemWidth = 70;
    int paddingHeight = 10;

    public delegate void BuyItemDelegate(ItemBlock item);
    public event BuyItemDelegate OnBuyItem;

    public void SetListSize(int capacity)
    {
        if (capacity <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        int width = defaultItemWidth * maxRow + 30;
        int column = (capacity - 1) / maxRow + 1;
        int height = defaultItemWidth * column + paddingHeight;

        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        var layout = GetComponent<LayoutElement>();
        layout.preferredHeight = height;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag.GetComponent<ItemBlock>();
        OnBuyItem?.Invoke(droppedItemBlock);
    }
}
