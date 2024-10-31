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

    private void OnEnable()
    {
        SetItemDialog(); // itemPanelがアクティブ化されるたびに実行
    }

    private void SetItemDialog()
    {
        Debug.Log("SetItemDialog");
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        bool isFirstItem = true;

        foreach (var item in playerUnit.Battler.Inventory)
        {
            // ItemUnitのインスタンスを生成
            Debug.Log($"item:{item.Base.Name}");
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

            // ItemUnitのSetupメソッドでアイテムデータを設定
            itemUnit.Setup(item);

            if (isFirstItem)
            {
                targetItem(itemUnit, true);
                isFirstItem = false;  // 2回目以降は実行されないように設定
            }
            else
            {
                targetItem(itemUnit, false);
            }
        }
    }

    public void targetItem(ItemUnit target, bool focusFlg)
    {
        target.Targetfoucs(focusFlg);
    }
}
