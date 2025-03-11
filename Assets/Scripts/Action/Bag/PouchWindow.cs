using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PouchWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI pouchRatio;
    [SerializeField] InventoryDialog inventoryDialog;
    private List<ItemUnit> itemUnitList = new List<ItemUnit>();

    private Battler playerBattler;

    private int headHeight = 105;
    private int itemWidth = 70;
    int row = 5;
    int padding = 10;

    public void Start()
    {
        SetPouchSize();
    }

    private void OnEnable()
    {
        SetItemUnit(playerBattler.PouchList);
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemUnit droppedItemUnit = eventData.pointerDrag.GetComponent<ItemUnit>();

        if (droppedItemUnit != null)
        {
            AddItemUnit(droppedItemUnit.Item); // ポーチに追加
            inventoryDialog.RemoveItem(droppedItemUnit); // バックから削除
        }
    }

    public void SetUp(Battler battler)
    {
        playerBattler = battler;
        SetItemUnit(playerBattler.PouchList);
    }

    public void SetPouchSize()
    {
        int width = itemWidth * row + 30;
        int column = (playerBattler.Pouch.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void SetItemUnit(List<Item> items)
    {
        itemUnitList.Clear();

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            itemUnit.OnEndDragAction += ArrengeItemUnits;
            itemUnitList.Add(itemUnit);
        }
        ArrengeItemUnits();
    }

    public void AddItemUnit(Item item)
    {
        playerBattler.PouchList.Add(item);
        SetItemUnit(playerBattler.PouchList);
    }

    public void RemoveItem(ItemUnit itemUnit)
    {
        itemUnitList.Remove(itemUnit);
        Destroy(itemUnit.gameObject);
        ArrengeItemUnits();
        playerBattler.PouchList.Remove(itemUnit.Item);
    }

    public void ArrengeItemUnits()
    {
        itemUnitList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < itemUnitList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (i % row) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((i / row) * itemWidth + cardHalfWidth) - padding;
            itemUnitList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }
}
