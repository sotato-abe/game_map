using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PouchWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject itemBlockPrefab;  // ItemBlockのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI pouchRatio;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] BattleUnit playerUnit;

    private List<ItemBlock> itemUnitList = new List<ItemBlock>();
    private List<GameObject> blockList = new List<GameObject>();
    private Battler playerBattler;

    private int headHeight = 10;
    private int itemWidth = 70;
    int row = 5;

    public void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPouchSize();
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetItemBlock();
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
    }

    public void SetPouchSize()
    {
        int width = itemWidth * row + 20;
        int column = (playerBattler.Pouch.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag.GetComponent<ItemBlock>();

        if (droppedItemBlock != null)
        {
            // すでにポーチに同じアイテムがあるか確認
            if (droppedItemBlock.transform.parent == itemList.transform)
            {
                Debug.Log("ポーチ内のアイテムをドロップしても何もしない");
                return;
            }
            if (playerBattler.PouchList.Count >= playerBattler.Pouch.val)
            {
                Debug.Log("ポーチがいっぱいです。");
                return; // 追加しない
            }

            inventoryWindow.RemoveItem(droppedItemBlock); // バックから削除
            AddItemBlock(droppedItemBlock.Item); // ポーチに追加
        }
    }

    private void SetItemBlock()
    {
        itemUnitList.Clear();

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in playerBattler.PouchList)
        {
            GameObject itemUnitObject = Instantiate(itemBlockPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemBlock itemUnit = itemUnitObject.GetComponent<ItemBlock>();
            itemUnit.Setup(item);
            itemUnit.OnEndDragAction += ArrengeItemBlocks;
            itemUnitList.Add(itemUnit);
        }
        SetBlock();
        ArrengeItemBlocks();
    }

    private void SetBlock()
    {
        int blockNum = (row - (playerBattler.Pouch.val % row)) % row;
        Debug.Log($"test：{blockList}");
        blockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockList.Add(blockObject);
        }
    }

    public void ArrengeItemBlocks()
    {
        itemUnitList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < itemUnitList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (i % row) * itemWidth + cardHalfWidth;
            int yPosition = -((i / row) * itemWidth + cardHalfWidth);
            itemUnitList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        // 右下からブロックを配置
        for (int i = 0; i < blockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (playerBattler.Pouch.val % row + i) * itemWidth + cardHalfWidth;
            int yPosition = -((playerBattler.Pouch.val / row) * itemWidth + cardHalfWidth);
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
            Debug.Log("ttt");
        }
    }

    public void AddItemBlock(Item item)
    {
        playerBattler.PouchList.Add(item);
        SetItemBlock();
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
    }

    public void RemoveItem(ItemBlock itemUnit)
    {
        itemUnitList.Remove(itemUnit);
        Destroy(itemUnit.gameObject);
        ArrengeItemBlocks();
        playerBattler.PouchList.Remove(itemUnit.Item);
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
    }
}
