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
    [SerializeField] GameObject accessoryList;
    [SerializeField] EquipmentBlock equipmentBlockPrefab;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] BattleUnit playerUnit;

    List<Equipment> armEquipmentList = new List<Equipment>();

    private Battler playerBattler;

    private void Awake()
    {
        playerBattler = playerUnit.Battler;
        SetEquipmentList();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ドロップされたアイテムを装備します。");
        EquipmentBlock droppedEquipmentBlock = eventData.pointerDrag.GetComponent<EquipmentBlock>();

        if (droppedEquipmentBlock != null)
        {
            // すでに装備ウィンドウに同じアイテムがあるか確認
            if (playerBattler.EquipmentList.Contains(droppedEquipmentBlock.Equipment))
            {
                Debug.Log("アイテムはすでに装備しています。");
                return; // 追加しない
            }
            AddEquipment(droppedEquipmentBlock.Equipment); // 装備に追加
            inventoryWindow.RemoveEquipment(droppedEquipmentBlock); // バックから削除
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
                SetEquipmentBlock(slot, equipments[i]);
            }
        }
        SetArmEquipmentBlock();
    }

    // 外部から追加する際に使用する。
    public void AddEquipment(Equipment equipment)
    {
        playerBattler.EquipmentList.Add(equipment);
        if (equipment.EquipmentBase.EquipmentType == EquipmentType.Arm)
        {
            armEquipmentList.Add(equipment);
            SetArmEquipmentBlock();
        }
        else
        {
            EquipmentSlot slot = GetTargetSlot(equipment.EquipmentBase.EquipmentType);
            SetEquipmentBlock(slot, equipment);
        }
    }

    public void RemoveEquipment(EquipmentBlock equipmentBlock)
    {
        Equipment equipment = equipmentBlock.Equipment;
        playerBattler.EquipmentList.Remove(equipment);
        if (equipment.EquipmentBase.EquipmentType == EquipmentType.Arm)
        {
            armEquipmentList.Remove(equipment);
            SetArmEquipmentBlock();
        }
        else
        {
            EquipmentSlot slot = GetTargetSlot(equipment.EquipmentBase.EquipmentType);
            slot.ReSetSlot();
        }
    }

    private void ClearSlot(EquipmentSlot slot)
    {
        EquipmentBlock existing = slot.GetComponentInChildren<EquipmentBlock>();
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

    private void SetArmEquipmentBlock()
    {
        // アイテムの数が3個以上ある時に、後ろの2つ以外をバックに戻す。
        if (armEquipmentList.Count > 2)
        {
            for (int i = 0; i < armEquipmentList.Count - 2; i++)
            {
                playerBattler.EquipmentList.Remove(armEquipmentList[i]);
                playerBattler.BagItemList.Add(armEquipmentList[i]);
            }
            armEquipmentList.RemoveRange(0, armEquipmentList.Count - 2);
        }else if (armEquipmentList.Count == 0)
        {
            arm1.ReSetSlot();
            arm2.ReSetSlot();
        }
        //バックに残ったアイテムを装備する
        switch (armEquipmentList.Count)
        {
            case 1:
                SetEquipmentBlock(arm1, armEquipmentList[0]);
                arm2.ReSetSlot();
                break;
            case 2:
                SetEquipmentBlock(arm2, armEquipmentList[0]);
                SetEquipmentBlock(arm1, armEquipmentList[1]);
                break;
        }
    }


    private void SetEquipmentBlock(EquipmentSlot targetPosition, Equipment equipment)
    {
        // EquipmentBlock がすでに存在するか確認
        EquipmentBlock equipmentBlock = targetPosition.GetComponentInChildren<EquipmentBlock>();

        if (equipmentBlock != null)
        {
            // 既に装備されているものを外してバッグに戻す
            equipmentBlock.Setup(equipment);
        }
        else
        {
            // EquipmentBlock が存在しない場合のみ新規作成
            EquipmentBlock newEquipmentBlock = Instantiate(equipmentBlockPrefab, targetPosition.transform);
            newEquipmentBlock.transform.localScale = targetPosition.transform.localScale;
            newEquipmentBlock.Setup(equipment);
            newEquipmentBlock.OnEndDragAction += ArrengeEquipmentBlocks;
            targetPosition.ArrangeEquipmentBlock();
        }
    }

    private void ArrengeEquipmentBlocks()
    {
        // head, body, arm1, arm2, leg に追加された EquipmentBlock を整列
        Debug.Log("整列");
        head.ArrangeEquipmentBlock();
        body.ArrangeEquipmentBlock();
        arm1.ArrangeEquipmentBlock();
        arm2.ArrangeEquipmentBlock();
        leg.ArrangeEquipmentBlock();
    }
}
