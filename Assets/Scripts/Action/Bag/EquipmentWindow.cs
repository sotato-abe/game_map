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
        SetEquipmentList(playerBattler.Equipments);
    }

    public void SetEquipmentList(List<Equipment> equipments)
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            SetEquipment(equipments[i]);
        }
    }

    public void SetEquipment(Equipment equipment)
    {
        switch (equipment.Base.Type)
        {
            case EquipmentType.Head:
                SetEquipmentCard(head.gameObject, equipment);
                break;
            case EquipmentType.Body:
                SetEquipmentCard(body.gameObject, equipment);
                break;
            case EquipmentType.Arm:
                SetArmEquipmentCard(equipment);
                break;
            case EquipmentType.Leg:
                SetEquipmentCard(leg.gameObject, equipment);
                break;
            case EquipmentType.Accessory:
                break;
        }
    }

    private Equipment SetEquipmentCard(GameObject targetPosition, Equipment equipment)
    {
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
        }

        // 新しい装備をセット
        equipmentCard.Setup(equipment);

        return oldEquipment;
    }

    private void SetArmEquipmentCard(Equipment equipment)
    {
        Debug.Log($"equipment:{equipment.Base.Name}/{equipment.Base.Type}");

        if (equipment.Base.Type != EquipmentType.Arm)
        {
            Debug.Log("EquipmentType is not Arm.");
            return;
        }

        // arm1に装備する
        if (arm1.transform.childCount > 0)
        {
            // arm1に装備がある場合、それをarm2に移動
            EquipmentCard arm1Card = arm1.GetComponentInChildren<EquipmentCard>();
            if (arm1Card != null)
            {
                Equipment arm1Equipment = arm1Card.Equipment;
                Debug.Log($"Moving {arm1Equipment.Base.Name} from arm1 to arm2.");

                // arm2に装備がある場合はバッグに戻す
                if (arm2.transform.childCount > 0)
                {
                    EquipmentCard arm2Card = arm2.GetComponentInChildren<EquipmentCard>();
                    if (arm2Card != null)
                    {
                        Equipment arm2Equipment = arm2Card.Equipment;
                        Debug.Log($"Moving {arm2Equipment.Base.Name} from arm2 to bag.");
                        // arm2の装備をバッグに戻す
                        playerBattler.Equipments.Remove(arm2Equipment);
                        playerBattler.BagEquipmentList.Add(arm2Equipment);
                    }
                }

                // arm2にarm1の装備を移動
                SetEquipmentCard(arm2.gameObject, arm1Equipment);
            }
        }

        // arm1に新しい装備をセット
        SetEquipmentCard(arm1.gameObject, equipment);
    }
}
