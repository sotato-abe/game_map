using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsumableDialog : VariableDialog
{
    [SerializeField] private TextMeshProUGUI probability;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] EnegyIcon costPrefab;
    [SerializeField] Image targetImage;

    public void Setup(Item item)
    {
        if (item is Consumable consumable)
        {
            namePlate.SetName(item.Base.Name);
            description.text = item.Base.Description;
            probability.SetText(consumable.ConsumableBase.Probability.Value.ToString() + "%");
            ResetSkillList();
            SetEnegy(consumable.ConsumableBase.DamageList, true);
            SetEnegy(consumable.ConsumableBase.RecoveryList, false);
            SetEnchant(consumable.ConsumableBase.EnchantList);
            SetCost(consumable.CostList);
            TargetData targetData = TargetDatabase.Instance?.GetData(consumable.Attack.Target);
            targetImage.sprite = targetData.icon;
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
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
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
