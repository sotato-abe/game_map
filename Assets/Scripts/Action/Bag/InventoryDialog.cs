using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryDialog : MonoBehaviour, IDropHandler
{
    public UnityAction OnActionExecute;
    public UnityAction<ItemUnit> OnDropItemUnitAction;
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI bagRatio;
    [SerializeField] BattleUnit playerUnit;

    private int headHeight = 105;
    private int itemWidth = 70;
    int row = 10;
    int padding = 10;

    private List<ItemUnit> itemUnitList = new List<ItemUnit>();
    private List<GameObject> blockList = new List<GameObject>();
    private int selectedItem = 0;

    public void Start()
    {
        SetInventorySize();
    }

    // ItemUnitがドロップされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        ItemUnit droppedItemUnit = eventData.pointerDrag.GetComponent<ItemUnit>();

        if (droppedItemUnit != null)
        {
            AddItemUnit(droppedItemUnit.Item); // バッグに追加
            OnDropItemUnitAction?.Invoke(droppedItemUnit);
        }
    }

    //playerUnitのBagの数値に応じでInventoryDialogのサイズを変更
    public void SetInventorySize()
    {
        int width = itemWidth * row + 30;
        int column = (playerUnit.Battler.Bag.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void SetItemUnit()
    {
        // リストをクリア
        itemUnitList.Clear();

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (Item item in playerUnit.Battler.BagList)
        {
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            itemUnit.OnEndDragAction += ArrengeItemUnits; // 正しく登録
            itemUnitList.Add(itemUnit);

            if (itemNum == selectedItem)
            {
                itemUnit.SetTarget(true);
            }

            itemNum++;
        }
        SetBlock();
        ArrengeItemUnits();
    }

    // Bagの数値に応じてパネルの左下からブロックを配置する
    // Bagが8の場合、左下から2つブロックを配置する
    private void SetBlock()
    {
        int blockNum = row - playerUnit.Battler.Bag.val % row;
        blockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockList.Add(blockObject);
        }
    }

    //カードを整列させる
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

        // 右下からブロックを配置
        for (int i = 0; i < blockList.Count; i++)
        {
            int cardHalfWidth = itemWidth / 2;
            int xPosition = (playerUnit.Battler.Bag.val % row + i) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((playerUnit.Battler.Bag.val / row) * itemWidth + cardHalfWidth) - padding;
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
        Debug.Log(playerUnit.Battler.BagList.Count);
        bagRatio.text = $"{playerUnit.Battler.BagList.Count}/{playerUnit.Battler.Bag.val}";
    }

    public void AddItemUnit(Item item)
    {
        playerUnit.Battler.BagList.Add(item);
        SetItemUnit();
    }

    public void RemoveItem(ItemUnit itemUnit)
    {
        itemUnitList.Remove(itemUnit);
        Destroy(itemUnit.gameObject);
        playerUnit.Battler.BagList.Remove(itemUnit.Item);
        ArrengeItemUnits();
    }

    public void SelectItem(ArrowType type)
    {
        if (itemList.transform.childCount > 0)
        {
            int targetItem = selectedItem; // 初期値を設定

            switch (type)
            {
                case ArrowType.Up:
                    if (selectedItem >= 10)
                        targetItem = selectedItem - 10;
                    break;

                case ArrowType.Right:
                    if (selectedItem < itemList.transform.childCount - 1)
                        targetItem = selectedItem + 1;
                    break;

                case ArrowType.Down:
                    if (selectedItem <= itemList.transform.childCount - 10)
                        targetItem = selectedItem + 10;
                    break;

                case ArrowType.Left:
                    if (selectedItem > 0)
                        targetItem = selectedItem - 1;
                    break;
            }

            if (targetItem != selectedItem) // アイテムが変わる場合のみ処理
            {
                itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>().SetTarget(false);
                itemList.transform.GetChild(targetItem).GetComponent<ItemUnit>().SetTarget(true);
                selectedItem = targetItem;
            }
        }
        else
        {
            Debug.LogWarning("No items in the list.");
        }
    }

    public void UseItem()
    {
        if (itemList.transform.childCount > 0)
        {
            // 選択されたアイテムの ItemUnit を取得
            if (selectedItem >= 0 && selectedItem < itemList.transform.childCount)
            {
                // 選択されたアイテムの ItemUnit を取得
                var targetItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();

                if (targetItemUnit != null && targetItemUnit.Item != null) // ItemUnit とその Item が存在するかを確認
                {
                    playerUnit.Battler.TakeRecovery(targetItemUnit.Item.Base.RecoveryList);
                    playerUnit.Battler.BagList.Remove(targetItemUnit.Item);

                    selectedItem = Mathf.Clamp(selectedItem, 0, itemList.transform.childCount - 2);
                    if (selectedItem > 0)
                    {
                        var selectedItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
                        selectedItemUnit.SetTarget(false);
                    }

                    SetItemUnit();
                    playerUnit.UpdateEnegyUI();
                    OnActionExecute?.Invoke();
                }
                else
                {
                    Debug.LogWarning("No item found to use.");
                }
            }
            else
            {
                Debug.LogWarning("Selected item is out of bounds.");
            }
        }
        else
        {
            Debug.LogWarning("Selected item is out of bounds.");
        }
    }
}
