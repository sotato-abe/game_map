using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDialog : MonoBehaviour
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;

    private List<ItemUnit> inventory;
    int selectedItem = 0;

    private void SetItemUnit(List<Item> items)
    {
        ResetItemUnit();
        int itemNum = 0;
        foreach (var item in items)
        {
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            inventory.Add(itemUnit);

            if (itemNum == selectedItem)
            {
                itemUnit.Targetfoucs(true);
            }

            itemNum++;
        }
    }

    private void ResetItemUnit()
    {
        foreach (var item in inventory)
        {
            Destroy(item.gameObject);
        }
        inventory.Clear();
    }
}
