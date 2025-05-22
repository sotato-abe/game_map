using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] EquipmentSlot head;
    [SerializeField] EquipmentSlot body;
    [SerializeField] EquipmentSlot arm1;
    [SerializeField] EquipmentSlot arm2;
    [SerializeField] EquipmentSlot leg;
    [SerializeField] EquipmentSlot accessory1;
    [SerializeField] EquipmentSlot accessory2;
    [SerializeField] EquipmentSlot accessory3;
    [SerializeField] GameObject accessoryList;
    [SerializeField] ItemBlock itemBlockPrefab;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] BattleUnit playerUnit;

    List<Item> armEquipmentList = new List<Item>();
    List<Item> accessoryEquipmentList = new List<Item>();

    private Battler playerBattler;

    private void Awake()
    {
        playerBattler = playerUnit.Battler;
        SetEquipmentList();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemBlock droppedItemBlock = eventData.pointerDrag.GetComponent<ItemBlock>();

        if (droppedItemBlock.Item is Equipment equipment)
        {
            // すでに装備ウィンドウに同じアイテムがあるか確認
            if (playerBattler.EquipmentList.Contains(equipment))
            {
                Debug.Log("アイテムはすでに装備しています。");
                return; // 追加しない
            }
            if (playerBattler.BagItemList.Contains(droppedItemBlock.Item))
            {
                inventoryWindow.RemoveItem(droppedItemBlock);
            }
            AddEquipment(equipment); // 装備に追加
        }
    }

    // 装備中の装備ブロックを表示。
    public void SetEquipmentList()
    {
        //  装備リストを初期化
        head.ReSetSlot();
        body.ReSetSlot();
        arm1.ReSetSlot();
        arm2.ReSetSlot();
        leg.ReSetSlot();
        armEquipmentList.Clear();

        List<Equipment> equipments = playerBattler.EquipmentList;

        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].EquipmentBase.EquipmentType == EquipmentType.Arm)
            {
                armEquipmentList.Add(equipments[i]);

                continue;
            }
            else
            {
                EquipmentSlot slot = GetTargetSlot(equipments[i].EquipmentBase.EquipmentType);
                SetItemBlock(slot, equipments[i]);
            }
        }
        SetArmItemBlock();
    }

    // 外部から追加する際に使用する。
    public void AddEquipment(Item item)
    {
        if (item is Equipment equipment)
        {
            playerBattler.EquipmentList.Add(equipment);
            if (equipment.EquipmentBase.EquipmentType == EquipmentType.Arm)
            {
                armEquipmentList.Add(equipment);
                SetArmItemBlock();
            }
            else if (equipment.EquipmentBase.EquipmentType == EquipmentType.Accessory)
            {
                accessoryEquipmentList.Add(equipment);
                SetAccessoryItemBlock();
            }
            else
            {
                EquipmentSlot slot = GetTargetSlot(equipment.EquipmentBase.EquipmentType);
                SetItemBlock(slot, equipment);
            }
        }
    }

    public void RemoveItem(ItemBlock equipmentBlock)
    {
        Equipment equipment = equipmentBlock.Item as Equipment;
        playerBattler.EquipmentList.Remove(equipment);
        if (equipment.EquipmentBase.EquipmentType == EquipmentType.Arm)
        {
            armEquipmentList.Remove(equipment);
            SetArmItemBlock();
        }
        else if (equipment.EquipmentBase.EquipmentType == EquipmentType.Accessory)
        {
            accessoryEquipmentList.Remove(equipment);
            SetAccessoryItemBlock();
        }
        else
        {
            EquipmentSlot slot = GetTargetSlot(equipment.EquipmentBase.EquipmentType);
            slot.ReSetSlot();
        }
    }

    private void ClearSlot(EquipmentSlot slot)
    {
        ItemBlock existing = slot.GetComponentInChildren<ItemBlock>();
        if (existing != null)
        {
            Destroy(existing.gameObject);
        }
    }

    private EquipmentSlot GetTargetSlot(EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Head => head,
            EquipmentType.Body => body,
            EquipmentType.Arm => arm2.transform.childCount > 0 ? arm2 : arm1,
            EquipmentType.Leg => leg,
            _ => null,
        };
    }

    private void SetArmItemBlock()
    {
        // アイテムの数が3個以上ある時に、後ろの2つ以外をバックに戻す。
        if (armEquipmentList.Count > 2)
        {
            for (int i = 0; i < armEquipmentList.Count - 2; i++)
            {
                if (armEquipmentList[i] is Equipment equipment)
                {
                    playerBattler.BagItemList.Add(armEquipmentList[i]);
                    playerBattler.EquipmentList.Remove(equipment);
                }
            }
            armEquipmentList.RemoveRange(0, armEquipmentList.Count - 2);
        }
        else if (armEquipmentList.Count == 0)
        {
            arm1.ReSetSlot();
            arm2.ReSetSlot();
        }
        //バックに残ったアイテムを装備する
        switch (armEquipmentList.Count)
        {
            case 1:
                SetItemBlock(arm1, armEquipmentList[0]);
                arm2.ReSetSlot();
                break;
            case 2:
                SetItemBlock(arm2, armEquipmentList[0]);
                SetItemBlock(arm1, armEquipmentList[1]);
                break;
        }
    }

    private void SetAccessoryItemBlock()
    {
        // アイテムの数が3個以上ある時に、後ろの2つ以外をバックに戻す。
        if (accessoryEquipmentList.Count > 3)
        {
            for (int i = 0; i < accessoryEquipmentList.Count - 2; i++)
            {
                if (accessoryEquipmentList[i] is Equipment equipment)
                {
                    playerBattler.BagItemList.Add(accessoryEquipmentList[i]);
                    playerBattler.EquipmentList.Remove(equipment);
                }
            }
            accessoryEquipmentList.RemoveRange(0, accessoryEquipmentList.Count - 3);
        }
        else if (accessoryEquipmentList.Count == 0)
        {
            accessory1.ReSetSlot();
            accessory2.ReSetSlot();
            accessory3.ReSetSlot();
        }
        //バックに残ったアイテムを装備する
        switch (accessoryEquipmentList.Count)
        {
            case 1:
                SetItemBlock(accessory1, accessoryEquipmentList[0]);
                accessory2.ReSetSlot();
                accessory3.ReSetSlot();
                break;
            case 2:
                SetItemBlock(accessory1, accessoryEquipmentList[0]);
                SetItemBlock(accessory2, accessoryEquipmentList[1]);
                accessory3.ReSetSlot();
                break;
            case 3:
                SetItemBlock(accessory1, accessoryEquipmentList[0]);
                SetItemBlock(accessory2, accessoryEquipmentList[1]);
                SetItemBlock(accessory3, accessoryEquipmentList[2]);
                break;
        }
    }


    private void SetItemBlock(EquipmentSlot targetPosition, Item item)
    {
        // ItemBlock がすでに存在するか確認
        ItemBlock itemBlock = targetPosition.GetComponentInChildren<ItemBlock>();

        if (itemBlock != null)
        {
            // 既に装備されているものを外してバッグに戻す
            itemBlock.Setup(item);
        }
        else
        {
            // ItemBlock が存在しない場合のみ新規作成
            ItemBlock newItemBlock = Instantiate(itemBlockPrefab, targetPosition.transform);
            newItemBlock.transform.localScale = targetPosition.transform.localScale;
            newItemBlock.Setup(item);
            newItemBlock.OnEndDragAction += ArrengeItemBlocks;
            targetPosition.ArrangeItemBlock();
        }
    }

    private void ArrengeItemBlocks()
    {
        // head, body, arm1, arm2, leg に追加された ItemBlock を整列
        Debug.Log("整列");
        head.ArrangeItemBlock();
        body.ArrangeItemBlock();
        arm1.ArrangeItemBlock();
        arm2.ArrangeItemBlock();
        leg.ArrangeItemBlock();
        accessory1.ArrangeItemBlock();
        accessory2.ArrangeItemBlock();
        accessory3.ArrangeItemBlock();
    }
}
