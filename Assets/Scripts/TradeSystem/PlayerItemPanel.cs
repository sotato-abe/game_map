using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerItemPanel : Panel
{
    [SerializeField] PlayerUnit playerUnit;
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] CommandBlock commandBlockPrefab;
    [SerializeField] PlayerItemList bagList;
    [SerializeField] PlayerItemList pouchList;
    [SerializeField] PlayerItemList equipmentList;
    [SerializeField] PlayerItemList storageList;
    [SerializeField] PlayerItemList deckList;
    [SerializeField] SlidePanel itemScrollPanel;
    [SerializeField] SlidePanel commandScrollPanel;
    private void Start()
    {
        bagList.OnBuyItem += BuyAndAddBagItem; // アイテムをバッグに追加するイベント
        pouchList.OnBuyItem += BuyAndAddPouchItem; // アイテムをポーチに追加するイベント
        equipmentList.OnBuyItem += BuyAndAddEquipmentItem; // アイテムを装備に追加するイベント
    }
    public void SetUp()
    {
        bagList.SetListSize(playerUnit.Battler.ColBag.val);
        pouchList.SetListSize(playerUnit.Battler.ColPouch.val);
        equipmentList.SetListSize(7);
        storageList.SetListSize(playerUnit.Battler.ColStorage.val);
        deckList.SetListSize(playerUnit.Battler.ColMemory.val);
        SetPlayerItems(playerUnit.Battler.BagItemList?.Select(i => (Item)i).ToList(), bagList.itemList);
        SetPlayerItems(playerUnit.Battler.PouchList?.Select(i => (Item)i).ToList(), pouchList.itemList);
        SetPlayerItems(playerUnit.Battler.EquipmentList?.Select(i => (Item)i).ToList(), equipmentList.itemList);
        SetPlayerCommands(playerUnit.Battler.StorageList, storageList.itemList);
        SetPlayerCommands(playerUnit.Battler.DeckList, deckList.itemList);
    }

    public void ShowItemList()
    {
        itemScrollPanel.SetActive(true);
        commandScrollPanel.SetActive(false);
    }

    public void ShowCommandList()
    {
        itemScrollPanel.SetActive(false);
        commandScrollPanel.SetActive(true);
    }

    public void SetPlayerItems(List<Item> items, GameObject targetList)
    {
        foreach (Transform block in targetList.transform)
        {
            Destroy(block.gameObject);
        }
        foreach (Item item in items)
        {
            ItemBlock itemBlock = Instantiate(itemBlockPrefab, targetList.transform);
            itemBlock.OnEndDragAction += ArrengeItemBlocks;
            itemBlock.OnSellItem += SellItem;
            itemBlock.OnDeleteItem += DeleteItem; // アイテムを削除するためのイベント
            itemBlock.Setup(item);
        }
    }

    public void SetPlayerCommands(List<Command> commands, GameObject targetList)
    {
        foreach (Transform block in targetList.transform)
        {
            Destroy(block.gameObject);
        }
        foreach (Command command in commands)
        {
            CommandBlock commandBlock = Instantiate(commandBlockPrefab, targetList.transform);
            commandBlock.OnEndDragAction += ArrengeItemBlocks;
            commandBlock.OnSellCommand += SellCommand;
            commandBlock.OnDeleteCommand += DeleteCommand; // アイテムを削除するためのイベント
            commandBlock.Setup(command);
        }
    }

    private void BuyAndAddBagItem(ItemBlock itemBlock)
    {
        if (playerUnit.Battler.ColBag.val <= playerUnit.Battler.BagItemList.Count)
            return;
        if (playerUnit.Battler.Money < itemBlock.Item.Base.Price)
            return;
        playerUnit.Battler.Money -= itemBlock.Item.Base.Price; // アイテムの価格を引く
        playerUnit.Battler.BagItemList.Add(itemBlock.Item);
        itemBlock.RemoveItem();
        ArrangeItemBlocks();
    }
    private void BuyAndAddPouchItem(ItemBlock itemBlock)
    {
        if (playerUnit.Battler.ColBag.val <= playerUnit.Battler.BagItemList.Count)
            return;
        if (playerUnit.Battler.Money < itemBlock.Item.Base.Price)
            return;

        if (itemBlock.Item is Consumable consumable)
        {
            playerUnit.Battler.PouchList.Add(consumable);
            playerUnit.Battler.Money -= itemBlock.Item.Base.Price; // アイテムの価格を引く
            itemBlock.RemoveItem();
            ArrangeItemBlocks();
        }
    }
    private void BuyAndAddEquipmentItem(ItemBlock itemBlock)
    {
        if (playerUnit.Battler.ColBag.val <= playerUnit.Battler.BagItemList.Count)
            return;
        if (playerUnit.Battler.Money < itemBlock.Item.Base.Price)
            return;

        if (itemBlock.Item is Equipment equipment)
        {
            playerUnit.Battler.EquipmentList.Add(equipment);
            playerUnit.Battler.Money -= itemBlock.Item.Base.Price; // アイテムの価格を引く
            itemBlock.RemoveItem();
            ArrangeItemBlocks();
        }
    }

    public void ArrengeItemBlocks()
    {
        // それぞれのリストのGridLayoutGroupを再起動
        foreach (PlayerItemList list in new PlayerItemList[] { bagList, pouchList, equipmentList, storageList, deckList })
        {
            GridLayoutGroup grid = list.itemList.GetComponent<GridLayoutGroup>();
            if (grid != null)
            {
                grid.enabled = false;
                grid.enabled = true;
            }
        }
    }

    public void DeleteItem(Item item)
    {
        if (playerUnit.Battler.BagItemList.Contains(item))
            playerUnit.Battler.BagItemList.Remove(item);
        else if (item is Consumable consumable && playerUnit.Battler.PouchList.Contains(consumable))
            playerUnit.Battler.PouchList.Remove(consumable);
        else if (item is Equipment equipment && playerUnit.Battler.EquipmentList.Contains(equipment))
            playerUnit.Battler.EquipmentList.Remove(equipment);
        SetUp();
    }

    public void DeleteCommand(Command command)
    {
        if (playerUnit.Battler.StorageList.Contains(command))
            playerUnit.Battler.StorageList.Remove(command);
        else if (playerUnit.Battler.DeckList.Contains(command))
            playerUnit.Battler.DeckList.Remove(command);
        else if (playerUnit.Battler.RunTable.Contains(command))
            playerUnit.Battler.RunTable.Remove(command);
        SetUp();
    }

    public void SellItem(Item item)
    {
        playerUnit.Battler.Money += item.Base.Price; // コマンドの価格を加算
        DeleteItem(item);  // アイテムを削除する処理を呼び出す

        ArrangeItemBlocks();
    }

    public void SellCommand(Command command)
    {
        playerUnit.Battler.Money += command.Base.Price; // コマンドの価格を加算
        DeleteCommand(command);  // コマンドを削除する処理を呼び出す

        ArrangeItemBlocks();
    }

    private void ArrangeItemBlocks()
    {
        if (playerUnit.Battler is PlayerBattler playerBattler)
            playerBattler.UpdatePropertyPanel();
        SetUp();
    }
}
