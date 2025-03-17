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
    [SerializeField] EquipmentBlock equipmentPrefab;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] BattleUnit battlerUnit;

    List<Equipment> armEquipmentList = new List<Equipment>();

    private Battler playerBattler;

    private void Awake()
    {
        if (playerBattler == null)
        {
            Debug.LogWarning("playerBattler is not initialized.");
        }
        playerBattler = battlerUnit.Battler;
        SetEquipmentList();
    }

    public void OnDrop(PointerEventData eventData)
    {
        EquipmentBlock droppedEquipmentBlock = eventData.pointerDrag.GetComponent<EquipmentBlock>();

        if (droppedEquipmentBlock != null)
        {
            // すでにポーチに同じアイテムがあるか確認
            if (playerBattler.Equipments.Contains(droppedEquipmentBlock.Equipment))
            {
                Debug.Log("アイテムはすでに装備しています。");
                return; // 追加しない
            }
            AddEquipment(droppedEquipmentBlock.Equipment); // ポーチに追加
            inventoryWindow.RemoveEquipment(droppedEquipmentBlock); // バックから削除
        }
    }

    // すでに装備しているアイテムを表示する。
    public void SetEquipmentList()
    {
        //  装備リストを初期化
        head.ReSetSlot();
        body.ReSetSlot();
        arm1.ReSetSlot();
        arm2.ReSetSlot();
        leg.ReSetSlot();
        armEquipmentList.Clear();

        List<Equipment> equipments = playerBattler.Equipments;

        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].Base.Type == EquipmentType.Arm)
            {
                armEquipmentList.Add(equipments[i]);
                continue;
            }
            else
            {
                EquipmentSlot slot = GetTargetSlot(equipments[i].Base.Type);
                SetEquipmentBlock(slot, equipments[i]);
            }
        }
        SetArmEquipmentBlock();
    }

    // 外部から追加する際に使用する。
    public void AddEquipment(Equipment equipment)
    {
        playerBattler.Equipments.Add(equipment);
        if (equipment.Base.Type == EquipmentType.Arm)
        {
            armEquipmentList.Add(equipment);
            SetArmEquipmentBlock();
        }
        else
        {
            EquipmentSlot slot = GetTargetSlot(equipment.Base.Type);
            SetEquipmentBlock(slot, equipment);
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
        // アイテムの数が2個以上ある時に、後ろの2つ以外をバックに戻す。
        if (armEquipmentList.Count > 2)
        {
            for (int i = 0; i < armEquipmentList.Count - 2; i++)
            {
                playerBattler.Equipments.Remove(armEquipmentList[i]);
                playerBattler.BagEquipmentList.Add(armEquipmentList[i]);
            }
            armEquipmentList.RemoveRange(0, armEquipmentList.Count - 2);
        }
        //バックに残ったアイテムを装備する
        switch (armEquipmentList.Count)
        {
            case 1:
                SetEquipmentBlock(arm1, armEquipmentList[0]);
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
            EquipmentBlock newEquipmentBlock = Instantiate(equipmentPrefab, targetPosition.transform);
            newEquipmentBlock.transform.position = targetPosition.transform.position;
            newEquipmentBlock.transform.localScale = targetPosition.transform.localScale;
            newEquipmentBlock.Setup(equipment);
            newEquipmentBlock.OnEndDragAction += ArrengeEquipmentBlocks;
        }
    }

    private void ArrengeEquipmentBlocks()
    {
        // head, body, arm1, arm2, leg に追加された EquipmentBlock を整列
        head.ArrangeEquipmentBlock();
        body.ArrangeEquipmentBlock();
        arm1.ArrangeEquipmentBlock();
        arm2.ArrangeEquipmentBlock();
        leg.ArrangeEquipmentBlock();
    }
}
