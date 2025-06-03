using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentDialog : VariableDialog
{
    [SerializeField] private TextMeshProUGUI probability;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] GameObject statusList;
    [SerializeField] StatusDialogIcon statusPrefab;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnchantIcon enchantIcon;
    [SerializeField] EnegyIcon costPrefab;
    [SerializeField] Image targetImage;

    public void Setup(Item item)
    {
        if (item is Equipment equipment)
        {
            namePlate.SetName(equipment.EquipmentBase.Name);
            description.text = equipment.EquipmentBase.Description;
            probability.SetText(equipment.EquipmentBase.Probability.Value.ToString() + "%");
            SetSkillList();
            SetEnegy(equipment.EquipmentBase.DamageList, true);
            SetEnegy(equipment.EquipmentBase.RecoveryList, false);
            SetEnchant(equipment.EquipmentBase.EnchantList);
            SetCost(equipment.EquipmentBase.CostList);
            SetStatus(equipment);
            TargetData targetData = TargetDatabase.Instance?.GetData(equipment.Attack.Target);
            targetImage.sprite = targetData.icon;
            ResizeDialog();
        }
    }

    private void SetStatus(Equipment equipment)
    {
        int statusCount = 0;
        // statusList内のオブジェクトを削除
        foreach (Transform child in statusList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var enegy in equipment.EnegyList)
        {
            if (enegy.val == 0) continue;
            EnegyIcon statusObject = Instantiate(enegyPrefab, statusList.transform);
            statusObject.gameObject.SetActive(true);
            EnegyIcon statusUnit = statusObject.GetComponent<EnegyIcon>();
            statusUnit.SetCostIcon(enegy);
            statusUnit.SetColor(enegy.val < 0);
            statusCount++;
        }

        // statusList内にステータスを追加
        foreach (var status in equipment.StatusList)
        {
            if (status.val <= 0) continue;
            StatusDialogIcon statusObject = Instantiate(statusPrefab, statusList.transform);
            statusObject.gameObject.SetActive(true);
            StatusDialogIcon statusUnit = statusObject.GetComponent<StatusDialogIcon>();
            statusUnit.SetStatusIcon(status);
            statusCount++;
        }

        if (statusCount == 0)
        {
            // ステータスがない場合はステータスリストを非表示にする
            statusList.SetActive(false);
        }
        else
        {
            // ステータスがある場合はステータスリストを表示する
            statusList.SetActive(true);
        }
    }

    private void SetSkillList()
    {
        // skillList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetEnegy(List<Enegy> enegies, bool isDamage)
    {
        // attackList内に攻撃力を追加
        foreach (var enegy in enegies)
        {
            EnegyIcon enegyObject = Instantiate(enegyPrefab, enchantList.transform);
            enegyObject.gameObject.SetActive(true);
            EnegyIcon enegyUnit = enegyObject.GetComponent<EnegyIcon>();
            enegyUnit.SetCostIcon(enegy);
            enegyUnit.SetColor(isDamage);
        }
    }

    private void SetEnchant(List<Enchant> enchants)
    {
        // enchantList内にスキルを追加
        foreach (var enchant in enchants)
        {
            EnchantIcon enchantObject = Instantiate(enchantIcon, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();
            enchantUnit.SetEnchant(enchant);
        }
    }

    private void SetCost(List<Enegy> costs)
    {
        // costList内のオブジェクトを削除
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // costList内にコストを追加
        foreach (var cost in costs)
        {
            if (0 < cost.val)
            {
                EnegyIcon enegyObject = Instantiate(costPrefab, costList.transform);
                enegyObject.gameObject.SetActive(true);
                EnegyIcon enegyUnit = enegyObject.GetComponent<EnegyIcon>();
                enegyUnit.SetCostIcon(cost);
            }
        }
    }
}
