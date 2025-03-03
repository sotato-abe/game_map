using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDialog : MonoBehaviour
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;
    [SerializeField] BattleUnit playerUnit;

    private List<ItemUnit> itemUnitList = new List<ItemUnit>();
    int selectedItem = 0;

    public void SetItemUnit()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (Item item in playerUnit.Battler.Inventory)
        {
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
    }
}
