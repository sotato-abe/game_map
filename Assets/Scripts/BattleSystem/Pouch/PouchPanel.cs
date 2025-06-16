using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PouchPanel : Panel
{
    [SerializeField] ItemUnit itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI pouchRatio;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] AttackSystem attackSystem;

    private Battler playerBattler;

    int selectedItem = 0;
    private int paddingHeight = 20;
    private int defaultItemWidth = 70;
    int maxRow = 10;
    int padding = 10;
    private List<ItemUnit> itemBlockList = new List<ItemUnit>();
    private List<GameObject> blockingBlockList = new List<GameObject>();

    private void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
        SetItemUnit();
    }

    public void Update()
    {
        if (itemBlockList.Count == 0)
        {
            return; // アイテムがない場合は何もしない
        }
        if (attackSystem.ActivePlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UseItem();
            }
        }

        // TODO：ターン実行時にActiveになるようにする。
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectItem(ArrowType.Down);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectItem(ArrowType.Right);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectItem(ArrowType.Up);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectItem(ArrowType.Left);
            }
        }
    }

    public void SetPanelSize()
    {
        int row = playerBattler.Pouch.val < maxRow ? playerBattler.Pouch.val : maxRow;
        int column = (playerBattler.Pouch.val - 1) / row + 1;
        int height = defaultItemWidth * column + paddingHeight;
        int width = defaultItemWidth * row + 20;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    private void SetItemUnit()
    {
        itemBlockList.Clear();
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (var consumable in playerBattler.PouchList)
        {
            // ItemUnitのインスタンスを生成
            Item item = consumable;
            ItemUnit itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            itemBlockList.Add(itemUnit);

            if (itemNum == selectedItem)
            {
                itemUnit.SetTarget(true);
            }

            itemNum++;
        }
        SetBlock();
        pouchRatio.text = $"{playerBattler.PouchList.Count}/{playerBattler.Pouch.val}";
        ArrengeItemUnits();
    }

    private void SetBlock()
    {
        if (playerBattler.Pouch.val <= maxRow)
            return;

        int blockNum = maxRow - (playerBattler.Pouch.val % maxRow);
        blockingBlockList.Clear();

        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, itemList.transform);
            blockObject.gameObject.SetActive(true);
            blockingBlockList.Add(blockObject);
        }
    }
    //カードを整列させる
    public void ArrengeItemUnits()
    {
        itemBlockList.RemoveAll(item => item == null); // 破棄されたオブジェクトを削除

        for (int i = 0; i < itemBlockList.Count; i++)
        {
            int cardHalfWidth = defaultItemWidth / 2;
            int xPosition = (i % maxRow) * defaultItemWidth + cardHalfWidth + padding;
            int yPosition = -((i / maxRow) * defaultItemWidth + cardHalfWidth) - padding;
            itemBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        // 右下からブロックを配置
        for (int i = 0; i < blockingBlockList.Count; i++)
        {
            int cardHalfWidth = defaultItemWidth / 2;
            int xPosition = (playerBattler.Pouch.val % maxRow + i) * defaultItemWidth + cardHalfWidth + padding;
            int yPosition = -((playerBattler.Pouch.val / maxRow) * defaultItemWidth + cardHalfWidth) - padding;
            blockingBlockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }

    public void SelectItem(ArrowType type)
    {
        if (itemBlockList.Count > 0)
        {
            int targetItem = selectedItem; // 初期値を設定

            switch (type)
            {
                case ArrowType.Up:
                    if (selectedItem >= maxRow)
                        targetItem = selectedItem - maxRow;
                    break;

                case ArrowType.Right:
                    if (selectedItem < itemBlockList.Count - 1)
                        targetItem = selectedItem + 1;
                    break;

                case ArrowType.Down:
                    if (selectedItem <= itemBlockList.Count - maxRow)
                        targetItem = selectedItem + maxRow;
                    break;

                case ArrowType.Left:
                    if (selectedItem > 0)
                        targetItem = selectedItem - 1;
                    break;
            }

            if (targetItem != selectedItem) // アイテムが変わる場合のみ処理
            {
                itemBlockList[selectedItem].SetTarget(false);
                itemBlockList[targetItem].SetTarget(true);
                selectedItem = targetItem;
            }

        }
        else
        {
            Debug.LogWarning("Selected item is out of bounds.");
        }
    }

    public void UseItem()
    {
        if (itemList.transform.childCount == 0)
        {
            Debug.LogWarning("No items in the list.");
            return;
        }

        if (selectedItem < 0 || selectedItem >= itemList.transform.childCount)
        {
            Debug.LogWarning("Selected item is out of bounds.");
            return;
        }

        var targetItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
        if (targetItemUnit == null || targetItemUnit.Item == null)
        {
            Debug.LogWarning("Selected ItemUnit or its Item is null.");
            return;
        }

        if (targetItemUnit.Item is not Consumable consumable)
        {
            Debug.LogWarning("Selected item is not a consumable.");
            return;
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, itemList.transform.childCount - 2);
        var selectedItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
        List<Attack> attacks = new List<Attack>();
        attacks.Add(consumable.Attack);
        attackSystem.ExecuteBattlerAttack(playerBattler, attacks, true);
        playerBattler.PouchList.Remove(consumable);

        SetItemUnit();
        playerUnit.UpdateEnegyUI();
        OnActionExecute?.Invoke();
        isActive = false;
    }
}
