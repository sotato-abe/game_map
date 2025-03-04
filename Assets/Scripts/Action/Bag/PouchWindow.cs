using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PouchWindow : MonoBehaviour
{
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject itemList;
    private List<ItemUnit> itemUnitList = new List<ItemUnit>();

    public void SetItemUnit(List<Item> items)
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();
            itemUnit.Setup(item);
            itemUnitList.Add(itemUnit);
        }
    }
}
