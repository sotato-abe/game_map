using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PouchWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] ItemBlock itemBlockPrefab;  // ItemBlockのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI pouchRatio;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] BattleUnit playerUnit;

    private List<ItemBlock> itemBlockList = new List<ItemBlock>();
    private List<GameObject> blockingBlockList = new List<GameObject>();
    private Battler playerBattler;

    private int headHeight = 10;
    private int itemWidth = 70;
    int row = 10;

    public void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPouchSize();
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetPouchList();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag.GetComponent<ItemBlock>();

        if (droppedItemBlock.Item is Consumable consumable)
        {
            // すでにポーチウィンドウに同じアイテムがあるか確認
            if (droppedItemBlock.transform.parent == itemList.transform)
            {
                Debug.Log("ポーチのアイテムです。");
                return;
            }
            AddItem(droppedItemBlock.Item); // ポーチに追加
        }
    }

    public void SetPouchSize()
    {
        int width = itemWidth * row + 20;
        int column = (playerBattler.Pouch.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    private void SetPouchList()
    {
        itemBlockList.Clear();

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Consumable consumable in playerBattler.PouchList)
        {
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlock.gameObject.SetActive(true);
            itemBlock.Setup(consumable);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlockList.Add(itemBlock);
        }
        SetBlockingBlock();
        ArrengeItemBlocks();
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
    }

    private void SetBlockingBlock()
    {
        int blockNum = (row - (playerBattler.Pouch.val % row)) % row;
        blockingBlockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockingBlockList.Add(blockObject);
        }
    }

    public void ArrengeItemBlocks()
    {
        itemBlockList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < itemBlockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (i % row) * itemWidth + cardHalfWidth;
            int yPosition = -((i / row) * itemWidth + cardHalfWidth);
            itemBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        // 右下からブロックを配置
        for (int i = 0; i < blockingBlockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (playerBattler.Pouch.val % row + i) * itemWidth + cardHalfWidth;
            int yPosition = -((playerBattler.Pouch.val / row) * itemWidth + cardHalfWidth);
            blockingBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }

    public void AddItem(Item item)
    {
        if (item is Consumable consumable)
        {
            if (playerBattler.PouchList.Count >= playerBattler.Pouch.val)
            {
                Debug.Log("ポーチがいっぱいです。");
                return; // 追加しない
            }
            if (playerBattler.BagItemList.Contains(item))
            {
                inventoryWindow.RemoveItem(item);
            }
            playerBattler.PouchList.Add(consumable);
            SetPouchList();
        }
    }

    public void RemoveItem(Item item)
    {
        if (item is Consumable consumable)
        {
            playerBattler.PouchList.Remove(consumable);
            SetPouchList();
            pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
        }
    }
}
