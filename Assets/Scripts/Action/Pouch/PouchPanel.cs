using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PouchPanel : Panel
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] BattleUnit playerUnit;

    int selectedItem = 0;

    private void Start()
    {
    }

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetItemUnit();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    public void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectItem(true);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectItem(false);
            }

            if (executeFlg)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UseItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isActive = false;
                OnActionExit?.Invoke();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isActive = true;
            }
        }
    }

    private void SetItemUnit()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (var item in playerUnit.Battler.Pouch)
        {
            // ItemUnitのインスタンスを生成
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

            itemUnit.Setup(item);

            if (itemNum == selectedItem)
            {
                itemUnit.SetTarget(true);
            }

            itemNum++;
        }
    }

    public void SelectItem(bool selectDirection)
    {
        if (itemList.transform.childCount > 0)
        {
            // 選択方向に応じてインデックスを変更し、範囲内に収める
            int newSelectedItem = selectedItem + (selectDirection ? 1 : -1);
            // 選択がリストの範囲外に行こうとした時に、選択が一周するようにする
            newSelectedItem = (newSelectedItem + itemList.transform.childCount) % itemList.transform.childCount;

            if (selectedItem != newSelectedItem)
            {
                itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>().SetTarget(false);
                // 新しいアイテムを選択状態にする
                itemList.transform.GetChild(newSelectedItem).GetComponent<ItemUnit>().SetTarget(true);
                selectedItem = newSelectedItem;
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
                    playerUnit.Battler.TakeRecovery(targetItemUnit.Item.Base.RecoveryList);
                    playerUnit.Battler.Pouch.Remove(targetItemUnit.Item);

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
