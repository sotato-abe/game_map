using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour, IDropHandler
{
    public UnityAction OnActionExecute;
    public UnityAction<EquipmentBlock> OnDropEquipmentBlockAction;
    [SerializeField] GameObject itemBlockPrefab;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI bagRatio;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] PouchWindow pouchWindow;
    [SerializeField] BattleUnit playerUnit;

    private Battler playerBattler;

    private int headHeight = 20;
    private int itemWidth = 70;
    int row = 10;
    int padding = 10;

    private List<ItemBlock> itemBlockList = new List<ItemBlock>();
    private List<GameObject> blockingBlockList = new List<GameObject>();
    private int selectedItem = 0;

    public void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetBlock();
    }

    public void SetPanelSize()
    {
        int width = itemWidth * row + 30;
        int column = (playerBattler.Bag.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    // ItemBlockがドロップされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag.GetComponent<ItemBlock>();

        if (droppedItemBlock != null)
        {
            if (droppedItemBlock.transform.parent == itemList.transform)
            {
                return;
            }
            if (playerBattler.BagItemList.Count >= playerBattler.Bag.val)
            {
                Debug.Log("バッグがいっぱいです。");
                return; // 追加しない
            }

            if (droppedItemBlock.Item is Equipment equipment)
            {
                // 装備Windowからドロップされた場合
                if (playerBattler.EquipmentList.Contains(equipment))
                {
                    AddItem(droppedItemBlock.Item); // バッグに追加
                    equipmentWindow.RemoveItem(droppedItemBlock.Item);
                }
            }
            else if (droppedItemBlock.Item is Consumable consumable)
            {
                // ポーチWindowからドロップされた場合
                if (playerBattler.PouchList.Contains(consumable))
                {
                    AddItem(droppedItemBlock.Item); // バッグに追加
                    pouchWindow.RemoveItem(droppedItemBlock.Item);
                }
            }
        }
    }

    //playerUnitのBagの数値に応じでInventoryWindowのサイズを変更
    public void SetBlock()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        if (playerBattler.BagItemList.Count <= selectedItem)
        {
            selectedItem = playerBattler.BagItemList.Count - 1;
        }

        SetBagItemBlock();
        SetBlockingBlock();
        ArrengeItemBlocks();
    }

    public void SetBagItemBlock()
    {
        // リストをクリア
        itemBlockList.Clear();
        itemBlockList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除

        int itemNum = 0;
        foreach (Item item in playerBattler.BagItemList)
        {
            GameObject itemBlockObject = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlockObject.gameObject.SetActive(true);
            ItemBlock itemBlock = itemBlockObject.GetComponent<ItemBlock>();
            itemBlock.Setup(item);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlockList.Add(itemBlock);

            if (itemNum == selectedItem)
            {
                itemBlock.SetTarget(true);
            }
            else
            {
                itemBlock.SetTarget(false);
            }
            itemNum++;
        }
    }

    private void SetBlockingBlock()
    {
        int blockNum = (row - (playerBattler.Bag.val % row)) % row;
        blockingBlockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockingBlockList.Add(blockObject);
        }
    }

    //カードを整列させる
    public void ArrengeItemBlocks()
    {
        // まとめたリストで描画処理
        for (int i = 0; i < itemBlockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (i % row) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((i / row) * itemWidth + cardHalfWidth) - padding;
            itemBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        // 右下からブロックを配置
        for (int i = 0; i < blockingBlockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (playerBattler.Bag.val % row + i) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((playerBattler.Bag.val / row) * itemWidth + cardHalfWidth) - padding;
            blockingBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
        int itemCount = playerBattler.BagItemList.Count;
        bagRatio.text = $"{itemCount}/{playerBattler.Bag.val}";
    }

    public void AddItem(Item item)
    {
        playerBattler.BagItemList.Add(item);
        SetBlock();
    }

    public void RemoveItem(Item item)
    {
        playerBattler.BagItemList.Remove(item);
        SetBlock();
    }

    public void SelectItem(ArrowType type)
    {
        int itemCount = itemBlockList.Count;
        if (itemCount == 0)
        {
            Debug.LogWarning("No items in the list.");
            return;
        }

        int targetItem = selectedItem;  // 初期値を設定

        switch (type)
        {
            case ArrowType.Up:
                if (selectedItem >= 10) targetItem -= 10;
                break;
            case ArrowType.Right:
                if (selectedItem < itemCount - 1) targetItem += 1;
                break;
            case ArrowType.Down:
                if (selectedItem <= itemCount - 10) targetItem += 10;
                break;
            case ArrowType.Left:
                if (selectedItem > 0) targetItem -= 1;
                break;
        }

        if (targetItem != selectedItem) // アイテムが変わる場合のみ処理
        {
            itemBlockList[selectedItem].SetTarget(false);
            itemBlockList[targetItem].SetTarget(true);
            selectedItem = targetItem;
        }
    }

    public void UseItem()
    {
        if (itemBlockList.Count > 0)
        {
            if (itemBlockList[selectedItem].Item is Consumable consumable)
            {
                playerBattler.TakeEnegy(consumable.ConsumableBase.RecoveryList, false);
                playerBattler.TakeEnchant(consumable.ConsumableBase.EnchantList);
                playerBattler.BagItemList.Remove(consumable);
                playerUnit.TakeEnchant(consumable.ConsumableBase.EnchantList);
                playerUnit.UpdateEnegyUI();
                SetBlock();
            }
            else if (itemBlockList[selectedItem].Item is Equipment)
            {
                // 装備品を装備する
                equipmentWindow.AddItem(itemBlockList[selectedItem].Item);
                SetBlock();
            }
            else
            {
                Debug.LogWarning("選択中のアイテムは使用できません。");
            }
        }
        else
        {
            Debug.LogWarning("使用できるアイテムがありません。");
        }
    }
}
