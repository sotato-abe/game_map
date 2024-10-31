using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] BattleUnit playerUnit;

    int previousItem;
    int selectedItem;

    private void Init()
    {
        selectedItem = 0;
        previousItem = selectedItem;
    }

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetItemDialog();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    private void SetItemDialog()
    {
        Debug.Log("SetItemDialog");

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (var item in playerUnit.Battler.Inventory)
        {
            // ItemUnitのインスタンスを生成
            Debug.Log($"item:{item.Base.Name}");
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

             if (itemNum == selectedItem)
            {
                itemUnit.Targetfoucs(true);
            }

            itemNum++;
        }
    }
    public void SelectItem(bool selectDirection)
    {
        if (selectDirection)
        {
            selectedItem++;
        }
        else
        {
            selectedItem--;
        }
        selectedItem = Mathf.Clamp(selectedItem, 0, playerUnit.Battler.Inventory.Count - 1);

        if (itemList.transform.childCount > 0 && previousItem != selectedItem)
        {
            var previousItemUnit = itemList.transform.GetChild(previousItem).GetComponent<ItemUnit>();
            previousItemUnit.Targetfoucs(false);
            var currentItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
            currentItemUnit.Targetfoucs(true);
            previousItem = selectedItem;
        }

        Debug.Log($"selectItem:{selectedItem}");
    }
}
