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

    int selectedItem;

    private void Init()
    {
        selectedItem = 0;
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
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        int itemNum = 0;

        foreach (var item in playerUnit.Battler.Inventory)
        {
            // ItemUnitのインスタンスを生成
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

            itemUnit.Setup(item);

            if (itemNum == selectedItem)
            {
                itemUnit.Targetfoucs(true);
            }

            itemNum++;
        }
    }
    public void SelectItem(bool selectDirection)
    {
        // 現在選択中のアイテムがリスト内に存在するか確認
        if (selectedItem >= 0 && selectedItem < itemList.transform.childCount)
        {
            var previousItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
            previousItemUnit.Targetfoucs(false);
        }

        // 選択方向に応じてインデックスを変更
        if (selectDirection)
        {
            selectedItem++;
        }
        else
        {
            selectedItem--;
        }

        // インデックスをリストの範囲内に制限
        selectedItem = Mathf.Clamp(selectedItem, 0, itemList.transform.childCount - 1);

        // 新しい選択中のアイテムをハイライト
        if (selectedItem >= 0 && selectedItem < itemList.transform.childCount)
        {
            var selectedItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
            selectedItemUnit.Targetfoucs(true);
        }
    }

    public void UseItem()
    {
        // 選択されたアイテムの ItemUnit を取得
        if (selectedItem >= 0 && selectedItem < itemList.transform.childCount)
        {
            // 選択されたアイテムの ItemUnit を取得
            var targetItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();

            if (targetItemUnit != null && targetItemUnit.Item != null) // ItemUnit とその Item が存在するかを確認
            {
                // アイテムを使用する処理をここに追加
                // 例: playerUnit.Battler.UseItem(targetItemUnit.Item);
                int MaxBattery = playerUnit.Battler.MaxBattery;
                int battery = playerUnit.Battler.Battery + targetItemUnit.Item.Base.Battery;
                battery = Mathf.Clamp(battery, 0, MaxBattery);

                int maxLife = playerUnit.Battler.MaxLife;
                int life = playerUnit.Battler.Life + targetItemUnit.Item.Base.Life;
                life = Mathf.Clamp(life, 0, maxLife);

                playerUnit.Battler.Life = life;
                playerUnit.Battler.Battery = battery;
                playerUnit.Battler.Inventory.Remove(targetItemUnit.Item);

                selectedItem = Mathf.Clamp(selectedItem, 0, itemList.transform.childCount - 2);

                var selectedItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();
                selectedItemUnit.Targetfoucs(false);

                SetItemDialog();
                playerUnit.UpdateUI();
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
}
