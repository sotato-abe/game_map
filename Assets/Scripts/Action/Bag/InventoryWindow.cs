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
    [SerializeField] GameObject equipmentBlockPrefab;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI bagRatio;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] BattleUnit playerUnit;

    private Battler playerBattler;

    private int headHeight = 97;
    private int itemWidth = 70;
    int row = 10;
    int padding = 10;
    int itemNum = 0;

    private List<ItemBlock> itemBlockList = new List<ItemBlock>();
    private List<EquipmentBlock> equipmentBlockList = new List<EquipmentBlock>();
    private List<GameObject> blockList = new List<GameObject>();
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
        EquipmentBlock droppedEquipmentBlock = eventData.pointerDrag.GetComponent<EquipmentBlock>();

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
        if (droppedEquipmentBlock != null)
        {
            if (droppedEquipmentBlock.transform.parent == itemList.transform)
            {
                Debug.Log("バッグ内の装備をドロップしても何もしない");
                return;
            }
            if (playerBattler.BagItemList.Count >= playerBattler.Bag.val)
            {
                Debug.Log("バッグがいっぱいです。");
                return; // 追加しない
            }

            AddEquipmentBlock(droppedEquipmentBlock.Equipment); // バッグに追加
            OnDropEquipmentBlockAction?.Invoke(droppedEquipmentBlock);
        }
    }

    //playerUnitのBagの数値に応じでInventoryWindowのサイズを変更
    public void SetItem()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        itemNum = 0;
        SetItemBlock();
        SetEquipmentBlock();
        SetBlock();
        ArrengeItemBlocks();

        if (0 < itemNum && selectedItem == 0)
        {
            if (itemBlockList.Count > 0)
            {
                itemBlockList[0].SetTarget(true);
            }
            else if (equipmentBlockList.Count > 0)
            {
                equipmentBlockList[0].SetTarget(true);
            }
        }
        else if (0 < itemNum && selectedItem > 0)
        {
            if (selectedItem < itemBlockList.Count)
            {
                itemBlockList[selectedItem].SetTarget(true);
            }
            else
            {
                equipmentBlockList[selectedItem - itemBlockList.Count].SetTarget(true);
            }
        }
    }

    public void SetItemBlock()
    {
        // リストをクリア
        itemBlockList.Clear();

        foreach (Item item in playerBattler.BagItemList)
        {
            itemNum++;

            GameObject itemBlockObject = Instantiate(itemBlockPrefab, itemList.transform);
            itemBlockObject.gameObject.SetActive(true);
            ItemBlock itemBlock = itemBlockObject.GetComponent<ItemBlock>();
            itemBlock.Setup(item);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlockList.Add(itemBlock);
        }
    }

    public void SetEquipmentBlock()
    {
        // リストをクリア
        equipmentBlockList.Clear();

        foreach (Equipment equipment in playerBattler.BagEquipmentList)
        {
            itemNum++;

            GameObject equipmentBlockObject = Instantiate(equipmentBlockPrefab, itemList.transform);
            equipmentBlockObject.gameObject.SetActive(true);
            EquipmentBlock equipmentBlock = equipmentBlockObject.GetComponent<EquipmentBlock>();
            equipmentBlock.Setup(equipment);
            equipmentBlock.OnEndDragAction += ArrengeItemBlocks;
            equipmentBlockList.Add(equipmentBlock);
        }
    }

    // Bagの数値に応じてパネルの左下からブロックを配置する
    // Bagが8の場合、左下から2つブロックを配置する
    private void SetBlock()
    {
        int blockNum = (row - (playerBattler.Bag.val % row)) % row;
        blockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockList.Add(blockObject);
        }
    }

    //カードを整列させる
    public void ArrengeItemBlocks()
    {
        itemBlockList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除
        equipmentBlockList.RemoveAll(equipment => equipment == null); // 念のため Equipment も削除

        // ItemとEquipmentをまとめたリストを作成
        List<GameObject> combinedList = new List<GameObject>();

        // itemBlockList から GameObject を追加
        foreach (var itemBlock in itemBlockList)
        {
            combinedList.Add(itemBlock.gameObject);
        }

        // equipmentBlockList から GameObject を追加
        foreach (var equipmentBlock in equipmentBlockList)
        {
            combinedList.Add(equipmentBlock.gameObject);
        }

        // まとめたリストで描画処理
        for (int i = 0; i < combinedList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (i % row) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((i / row) * itemWidth + cardHalfWidth) - padding;
            combinedList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        // 右下からブロックを配置
        for (int i = 0; i < blockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (playerBattler.Bag.val % row + i) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((playerBattler.Bag.val / row) * itemWidth + cardHalfWidth) - padding;
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
        int itemCount = playerBattler.BagItemList.Count + playerBattler.BagEquipmentList.Count;
        bagRatio.text = $"{itemCount}/{playerBattler.Bag.val}";
    }

    public void AddItemBlock(Item item)
    {
        playerBattler.BagItemList.Add(item);
        SetItemBlock();
        ArrengeItemBlocks();
    }

    public void AddEquipmentBlock(Equipment equipment)
    {
        playerBattler.BagEquipmentList.Add(equipment);
        SetEquipmentBlock();
        ArrengeItemBlocks();
    }

    public void RemoveItem(ItemBlock itemBlock)
    {
        playerBattler.BagItemList.Remove(itemBlock.Item);
        SetItemBlock();
        ArrengeItemBlocks();
    }

    public void RemoveEquipment(EquipmentBlock equipmentBlock)
    {
        playerBattler.BagEquipmentList.Remove(equipmentBlock.Equipment);
        SetItem();
    }

    public void SelectItem(ArrowType type)
    {
        int itemCount = itemBlockList.Count + equipmentBlockList.Count;
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
            if (selectedItem < itemBlockList.Count)
            {
                itemBlockList[selectedItem].SetTarget(false);
            }
            else
            {
                equipmentBlockList[selectedItem - itemBlockList.Count].SetTarget(false);
            }

            if (targetItem < itemBlockList.Count)
            {
                itemBlockList[targetItem].SetTarget(true);
            }
            else
            {
                equipmentBlockList[targetItem - itemBlockList.Count].SetTarget(true);
            }
            selectedItem = targetItem;
        }
    }

    public void UseItem()
    {
        if (itemList.transform.childCount > 0)
        {
            if (selectedItem >= 0 && selectedItem < itemBlockList.Count)
            {
                // アイテムを使用する
                var targetItemBlock = itemList.transform.GetChild(selectedItem).GetComponent<ItemBlock>();
                UseItemBlock(targetItemBlock);
            }
            else if (itemBlockList.Count <= selectedItem && selectedItem < itemList.transform.childCount)
            {
                // 装備品を装備する
                var targetEquipmentBlock = itemList.transform.GetChild(selectedItem).GetComponent<EquipmentBlock>();
                UseEquipmentUnit(targetEquipmentBlock);
            }
            else
            {
                Debug.LogWarning("Selected item is out of bounds.");
            }
            int itemCount = playerBattler.BagItemList.Count + playerBattler.BagEquipmentList.Count;
            if (selectedItem >= itemCount)
            {
                selectedItem = itemCount - 1;
            }
            SetItem();
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
            playerBattler.TakeRecovery(itemBlock.Item.Base.RecoveryList);
            playerBattler.BagItemList.Remove(itemBlock.Item);
            SetItem();
            playerUnit.UpdateEnegyUI();
        }
        else
        {
            Debug.LogWarning($"No item found to use.{selectedItem}");
        }
    }

    private void UseEquipmentUnit(EquipmentBlock targetEquipmentBlock)
    {
        if (targetEquipmentBlock != null && targetEquipmentBlock.Equipment != null)
        {
            string name = targetEquipmentBlock.Equipment.Base.Name;
            playerBattler.BagEquipmentList.Remove(targetEquipmentBlock.Equipment);
            equipmentWindow.AddEquipment(targetEquipmentBlock.Equipment);
        }
        else
        {
            Debug.LogWarning("No equipment found to use.");
        }
    }
}
