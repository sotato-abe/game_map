using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopPanel : Panel
{
    [SerializeField] ItemBlock itemBlockPrefab;  // ItemBlockのプレハブ
    [SerializeField] CommandBlock commandBlockPrefab;  // CommandBlockのプレハブ
    [SerializeField] GameObject consumableList;
    [SerializeField] GameObject equipmentList;
    [SerializeField] GameObject treasureList;
    [SerializeField] GameObject commandList;

    public void SetUp(BuildingBase building)
    {
        SetShopItems(building.ConsumableItems?.Select(i => (Item)i).ToList(), consumableList);
        SetShopItems(building.EquipmentItems?.Select(i => (Item)i).ToList(), equipmentList);
        SetShopItems(building.TreasureItems?.Select(i => (Item)i).ToList(), treasureList);
        SetShopCommands(building.CommandItems);
    }

    public void SetShopItems(List<Item> items, GameObject targetList)
    {
        foreach (Transform block in targetList.transform)
        {
            Destroy(block.gameObject);
        }
        foreach (Item item in items)
        {
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, targetList.transform);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlock.OnSellItem += DeleteItem; // アイテムを売るためのイベント
            itemBlock.OnDeleteItem += DeleteItem; // アイテムを削除するためのイベント
            itemBlock.Setup(item);
        }
    }

    public void SetShopCommands(List<Command> commands)
    {
        foreach (Transform block in commandList.transform)
        {
            Destroy(block.gameObject);
        }
        foreach (Command command in commands)
        {
            CommandBlock commandBlock = Instantiate(commandBlockPrefab, commandList.transform);
            commandBlock.OnEndDragAction += ArrengeItemBlocks;
            commandBlock.Setup(command);
        }
    }

    public void ArrengeItemBlocks()
    {
        // それぞれのリストのGridLayoutGroupを再起動
        foreach (GameObject list in new GameObject[] { consumableList, equipmentList, treasureList, commandList })
        {
            GridLayoutGroup grid = list.GetComponent<GridLayoutGroup>();
            if (grid != null)
            {
                grid.enabled = false;
                grid.enabled = true; // 再起動
            }
        }
    }

    public void DeleteItem(Item item)
    {
        Transform listTransform = null;

        if (item is Consumable)
            listTransform = consumableList.transform;
        else if (item is Equipment)
            listTransform = equipmentList.transform;
        else if (item is Treasure)
            listTransform = treasureList.transform;
        else
        {
            return;
        }

        foreach (Transform child in listTransform)
        {
            ItemBlock block = child.GetComponent<ItemBlock>();
            if (block != null && block.Item == item)
            {
                Destroy(child.gameObject);
                break; // 一致するのが1個だけなら break でOK
            }
        }
    }
}
