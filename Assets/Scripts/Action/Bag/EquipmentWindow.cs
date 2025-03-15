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
    [SerializeField] EquipmentCard equipmentPrefab;
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
        Debug.Log("OnDrop");
        EquipmentCard droppedEquipmentCard = eventData.pointerDrag.GetComponent<EquipmentCard>();

        if (droppedEquipmentCard != null)
        {
            // すでにポーチに同じアイテムがあるか確認
            if (playerBattler.Equipments.Contains(droppedEquipmentCard.Equipment))
            {
                Debug.Log("アイテムはすでに装備しています。");
                return; // 追加しない
            }
            AddEquipment(droppedEquipmentCard.Equipment); // ポーチに追加
            inventoryWindow.RemoveEquipment(droppedEquipmentCard); // バックから削除
        }
    }

    public void SetEquipmentList()
    {
        Debug.Log("Equipments: " + playerBattler.Equipments.Count);
        //  装備リストを初期化
        head.ReSetSlot();
        body.ReSetSlot();
        arm1.ReSetSlot();
        arm2.ReSetSlot();
        leg.ReSetSlot();

        List<Equipment> equipments = playerBattler.Equipments;
        for (int i = 0; i < equipments.Count; i++)
        {
            AddEquipment(equipments[i]);
        }
    }

    public void AddEquipment(Equipment equipment)
    {
        if (equipment.Base.Type == EquipmentType.Arm)
        {
            SetArmTargetSlot(equipment);
            return;
        }
        EquipmentSlot slot = GetTargetSlot(equipment.Base.Type);
        SetEquipmentCard(slot.gameObject, equipment);
    }

    private EquipmentSlot GetTargetSlot(EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Head => head,
            EquipmentType.Body => body,
            EquipmentType.Arm => arm1.transform.childCount > 0 ? arm2 : arm1,
            EquipmentType.Leg => leg,
            _ => null,
        };
    }

    private void SetArmTargetSlot(Equipment equipment)
    {
        armEquipmentList.Add(equipment);
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
        for (int i = 0; i < armEquipmentList.Count; i++)
        {
            EquipmentSlot slot = i == 0 ? arm2 : arm1;
            SetEquipmentCard(slot.gameObject, armEquipmentList[i]);
        }
    }


    private void SetEquipmentCard(GameObject targetPosition, Equipment equipment)
    {
        Debug.Log($"SetEquipmentCard:{equipment.Base.Name}/{equipment.Base.Type}");
        // EquipmentCard がすでに存在するか確認
        EquipmentCard equipmentCard = transform.GetComponentInChildren<EquipmentCard>() ;

        if (equipmentCard != null)
        {
            Debug.Log("oldEquipment: " + equipmentCard.Equipment.Base.Name);
            // 既に装備されているものを外してバッグに戻す
            equipmentCard.Setup(equipment);
        }
        else
        {
            // EquipmentCard が存在しない場合のみ新規作成
            EquipmentCard newEquipmentCard = Instantiate(equipmentPrefab, targetPosition.transform);
            newEquipmentCard.transform.position = targetPosition.transform.position;
            newEquipmentCard.transform.localScale = targetPosition.transform.localScale;
            newEquipmentCard.OnEndDragAction += ArrengeEquipmentCards;
        }
    }

    private void ArrengeEquipmentCards()
    {
        Debug.Log("ArrengeEquipmentCards");
        // head, body, arm1, arm2, leg に追加された EquipmentCard を整列
        head.ArrangeEquipmentCard();
        body.ArrangeEquipmentCard();
        arm1.ArrangeEquipmentCard();
        arm2.ArrangeEquipmentCard();
        leg.ArrangeEquipmentCard();
    }
}
