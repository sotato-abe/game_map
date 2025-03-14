using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentWindow : MonoBehaviour
{
    [SerializeField] EquipmentSlot head;
    [SerializeField] EquipmentSlot body;
    [SerializeField] EquipmentSlot arm1;
    [SerializeField] EquipmentSlot arm2;
    [SerializeField] EquipmentSlot leg;
    [SerializeField] GameObject accessoryList;
    [SerializeField] EquipmentCard equipmentPrefab;  // ItemUnitのプレハブ
    [SerializeField] BattleUnit battlerUnit;

    private Battler playerBattler;

    private void Start()
    {
        if (playerBattler == null)
        {
            Debug.LogWarning("playerBattler is not initialized.");
        }
        playerBattler = battlerUnit.Battler;
        SetEquipmentList();
    }

    public void SetEquipmentList()
    {
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
        Debug.Log($"SetArmEquipmentCard:{equipment.Base.Name}/{equipment.Base.Type}");

        if (equipment.Base.Type != EquipmentType.Arm)
        {
            Debug.Log("EquipmentType is not Arm.");
            return;
        }

        // arm1 の EquipmentCard を取得（無ければ新規作成）
        EquipmentCard arm1Card = arm1.GetComponentInChildren<EquipmentCard>();
        if (arm1Card == null)
        {
            arm1Card = Instantiate(equipmentPrefab, arm1.transform);
            arm1Card.transform.position = arm1.transform.position;
            arm1Card.transform.localScale = arm1.transform.localScale;
        }

        Equipment oldArm1Equipment = arm1Card.Equipment;
        arm1Card.Setup(equipment);

        if (oldArm1Equipment != null)
        {
            // arm2 の EquipmentCard を取得（無ければ新規作成）
            EquipmentCard arm2Card = arm2.GetComponentInChildren<EquipmentCard>();
            if (arm2Card == null)
            {
                arm2Card = Instantiate(equipmentPrefab, arm2.transform);
                arm2Card.transform.position = arm2.transform.position;
                arm2Card.transform.localScale = arm2.transform.localScale;
            }

            Equipment oldArm2Equipment = arm2Card.Equipment;
            arm2Card.Setup(oldArm1Equipment);

            // arm2 に古い装備があればバッグに戻す
            if (oldArm2Equipment != null)
            {
                playerBattler.Equipments.Remove(oldArm2Equipment);
                playerBattler.BagEquipmentList.Add(oldArm2Equipment);
            }
        }
    }


    private void SetEquipmentCard(GameObject targetPosition, Equipment equipment)
    {
        Debug.Log($"SetEquipmentCard:{equipment.Base.Name}/{equipment.Base.Type}");
        // EquipmentCard がすでに存在するか確認
        EquipmentCard equipmentCard = targetPosition.GetComponent<EquipmentCard>();

        // 既に装備されているものがあれば取得
        Equipment oldEquipment = equipmentCard != null ? equipmentCard.Equipment : null;

        if (oldEquipment != null)
        {
            Debug.Log("oldEquipment: " + oldEquipment.Base.Name);
            // 既に装備されているものを外してバッグに戻す
            playerBattler.Equipments.Remove(oldEquipment);
            playerBattler.BagEquipmentList.Add(oldEquipment);
        }
        else
        {
            // EquipmentCard が存在しない場合のみ新規作成
            equipmentCard = Instantiate(equipmentPrefab, targetPosition.transform);
            equipmentCard.transform.position = targetPosition.transform.position;
            equipmentCard.transform.localScale = targetPosition.transform.localScale;
        }

        // 新しい装備を Equipments に追加（重複防止）
        if (!playerBattler.Equipments.Contains(equipment))
        {
            playerBattler.Equipments.Add(equipment);
            // playerBattler.Equipments.Insert(0, equipment);
        }

        // 新しい装備をセット
        equipmentCard.Setup(equipment);

        Debug.Log($"SetEquipmentCard: {equipment.Base.Name}");
    }
}
