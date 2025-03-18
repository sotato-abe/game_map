using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PouchPanel : Panel
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI pouchRatio;
    [SerializeField] BattleUnit playerUnit;

    private Battler playerBattler;

    int selectedItem = 0;

    private int headHeight = 40;
    private int itemWidth = 70;
    int row = 10;
    int padding = 10;
    private List<ItemUnit> itemUnitList = new List<ItemUnit>();
    private List<GameObject> blockList = new List<GameObject>();

    private void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetItemUnit();
    }

    public void Update()
    {
        if (!isActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isActive = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isActive = false;
                OnActionExit?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectItem(ArrowType.Up);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectItem(ArrowType.Right);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectItem(ArrowType.Down);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectItem(ArrowType.Left);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UseItem();
            }
        }
    }

    public void SetPanelSize()
    {
        int width = itemWidth * row + 40;
        int column = (playerBattler.Pouch.val - 1) / row + 1;
        int height = itemWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    private void SetItemUnit()
    {
        itemUnitList.Clear();
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (var item in playerBattler.PouchList)
        {
            // ItemUnitのインスタンスを生成
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            itemUnitList.Add(itemUnit);

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
        int blockNum = row - playerBattler.Pouch.val % row;
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
            int xPosition = (playerBattler.Pouch.val % row + i) * itemWidth + cardHalfWidth + padding;
            int yPosition = -((playerBattler.Pouch.val / row) * itemWidth + cardHalfWidth) - padding;
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }

    public void SelectItem(ArrowType type)
    {
        if (itemUnitList.Count > 0)
        {
            int targetItem = selectedItem; // 初期値を設定

            switch (type)
            {
                case ArrowType.Up:
                    if (selectedItem >= row)
                        targetItem = selectedItem - row;
                    break;

                case ArrowType.Right:
                    if (selectedItem < itemUnitList.Count - 1)
                        targetItem = selectedItem + 1;
                    break;

                case ArrowType.Down:
                    if (selectedItem <= itemUnitList.Count - row)
                        targetItem = selectedItem + row;
                    break;

                case ArrowType.Left:
                    if (selectedItem > 0)
                        targetItem = selectedItem - 1;
                    break;
            }

            if (targetItem != selectedItem) // アイテムが変わる場合のみ処理
            {
                itemUnitList[selectedItem].SetTarget(false);
                itemUnitList[targetItem].SetTarget(true);
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
        if (itemList.transform.childCount > 0)
        {
            // 選択されたアイテムの ItemUnit を取得
            if (selectedItem >= 0 && selectedItem < itemList.transform.childCount)
            {
                // 選択されたアイテムの ItemUnit を取得
                var targetItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();

                if (targetItemUnit != null && targetItemUnit.Item != null) // ItemUnit とその Item が存在するかを確認
                {
                    isActive = false;
                    playerBattler.TakeRecovery(targetItemUnit.Item.Base.RecoveryList);
                    playerBattler.PouchList.Remove(targetItemUnit.Item);

                    selectedItem = Mathf.Clamp(selectedItem, 0, itemList.transform.childCount - 2);

                    var selectedItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
                    selectedItemUnit.SetTarget(false);

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
