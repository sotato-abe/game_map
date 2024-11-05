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
    }

    public void UseItem()
    {
        // 選択されたアイテムの ItemUnit を取得
        var targetItemUnit = itemList.transform.GetChild(selectedItem).GetComponent<ItemUnit>();

        if (targetItemUnit != null && targetItemUnit.Item != null) // ItemUnit とその Item が存在するかを確認
        {
            Debug.Log($"Using item: {targetItemUnit.Item.Base}");

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
            SetItemDialog();
            playerUnit.UpdateUI();
        }
        else
        {
            Debug.LogWarning("No item found to use.");
        }
    }
}
