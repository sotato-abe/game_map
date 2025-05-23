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
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnchantIcon enchantIcon;
    [SerializeField] EnegyIcon costPrefab;

    public void Setup(Item item)
    {
        if (item is Equipment equipment)
        {
            namePlate.SetName(equipment.EquipmentBase.Name);
            description.text = equipment.EquipmentBase.Description;
            probability.SetText(equipment.EquipmentBase.Probability.Value.ToString() + "%");
            ResetSkillList();
            SetEnegy(equipment.EquipmentBase.DamageList, true);
            SetEnegy(equipment.EquipmentBase.RecoveryList, false);
            SetEnchant(equipment.EquipmentBase.EnchantList);
            SetCost(equipment.EquipmentBase.CostList);
            ResizeDialog();
        }
    }

    private void ResetSkillList()
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
