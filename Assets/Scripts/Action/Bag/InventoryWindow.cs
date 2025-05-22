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
    public UnityAction<ItemBlock> OnDropItemBlockAction;
    public UnityAction<EquipmentBlock> OnDropEquipmentBlockAction;
    [SerializeField] GameObject itemBlockPrefab;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI bagRatio;
    [SerializeField] EquipmentWindow equipmentWindow;
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
        SetItem();
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
                Debug.Log("バッグ内のアイテムをドロップしても何もしない");
                return;
            }
            if (playerBattler.BagItemList.Count >= playerBattler.Bag.val)
            {
                Debug.Log("バッグがいっぱいです。");
                return; // 追加しない
            }

            OnDropItemBlockAction?.Invoke(droppedItemBlock);
            AddItemBlock(droppedItemBlock.Item); // バッグに追加
        }
    }

    //playerUnitのBagの数値に応じでInventoryWindowのサイズを変更
    public void SetItem()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        CreateBlockBagItemList();
        SetBlockingBlock();
        ArrengeItemBlocks();
    }

    public void CreateBlockBagItemList()
    {
        // リストをクリア
        itemBlockList.Clear();

        foreach (Item item in playerBattler.BagItemList)
        {
            GameObject itemBlockObject = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlockObject.gameObject.SetActive(true);
            ItemBlock itemBlock = itemBlockObject.GetComponent<ItemBlock>();
            itemBlock.Setup(item);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlockList.Add(itemBlock);
        }
    }

    // Bagの数値に応じてパネルの左下からブロックを配置する
    // Bagが8の場合、左下から2つブロックを配置する
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
        itemBlockList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除

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

    public void AddItemBlock(Item item)
    {
        playerBattler.BagItemList.Add(item);
        CreateBlockBagItemList();
        ArrengeItemBlocks();
    }

    public void AddEquipmentBlock(Equipment equipment)
    {
        playerBattler.BagItemList.Add(equipment);
        // SetEquipmentBlock();
        ArrengeItemBlocks();
    }

    public void RemoveItem(ItemBlock itemBlock)
    {
        playerBattler.BagItemList.Remove(itemBlock.Item);
        itemBlockList.Remove(itemBlock);
        Destroy(itemBlock.gameObject);
        CreateBlockBagItemList();
        ArrengeItemBlocks();
    }

    public void RemoveEquipment(EquipmentBlock equipmentBlock)
    {
        playerBattler.BagItemList.Remove(equipmentBlock.Equipment);
        SetItem();
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
        if (itemList.transform.childCount > 0)
        {
            // TODO:後でまとめて修正
            // if (selectedItem >= 0 && selectedItem < itemBlockList.Count)
            // {
            //     // アイテムを使用する
            //     var targetItemBlock = itemList.transform.GetChild(selectedItem).GetComponent<ItemBlock>();
            //     UseItemBlock(targetItemBlock);
            // }
            // else if (itemBlockList.Count <= selectedItem && selectedItem < itemList.transform.childCount)
            // {
            //     // 装備品を装備する
            //     var targetEquipmentBlock = itemList.transform.GetChild(selectedItem).GetComponent<EquipmentBlock>();
            //     UseEquipmentUnit(targetEquipmentBlock);
            // }
            // else
            // {
            //     Debug.LogWarning("Selected item is out of bounds.");
            // }
            // int itemCount = playerBattler.BagItemList.Count + playerBattler.BagEquipmentList.Count;
            // if (selectedItem >= itemCount)
            // {
            //     selectedItem = itemCount - 1;
            // }
            // SetItem();
            playerUnit.UpdateEnegyUI();
        }
        else
        {
            Debug.LogWarning("Selected item is out of bounds.");
        }
    }

    private void UseItemBlock(ItemBlock itemBlock)
    {
        if (itemBlock != null && itemBlock.Item != null) // ItemBlock とその Item が存在するかを確認
        {
            // TODO:後でまとめて修正
            // playerBattler.TakeRecovery(itemBlock.Item.Base.RecoveryList);
            // playerBattler.TakeEnchant(itemBlock.Item.Base.EnchantList);
            // playerBattler.BagItemList.Remove(itemBlock.Item);
            // SetItem();
            // playerUnit.TakeEnchant(itemBlock.Item.Base.EnchantList);
            playerUnit.UpdateEnegyUI();
        }
        else
        {
            Debug.LogWarning($"No item found to use.{selectedItem}");
        }
    }

    // private void UseEquipmentUnit(EquipmentBlock targetEquipmentBlock)
    // {
    //     if (targetEquipmentBlock != null && targetEquipmentBlock.Equipment != null)
    //     {
    //         string name = targetEquipmentBlock.Equipment.Base.Name;
    //         playerBattler.BagEquipmentList.Remove(targetEquipmentBlock.Equipment);
    //         equipmentWindow.AddEquipment(targetEquipmentBlock.Equipment);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("No equipment found to use.");
    //     }
    // }
}
