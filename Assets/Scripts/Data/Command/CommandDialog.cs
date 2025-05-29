using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandDialog : VariableDialog
{
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] EnegyIcon costPrefab;
    [SerializeField] Image targetImage;

    public void Setup(Command command)
    {
        namePlate.SetName(command.Base.Name);
        description.text = command.Base.Description;
        ResetSkillList();
        SetEnegy(command.Base.DamageList, true);
        SetEnegy(command.Base.RecoveryList, false);
        SetEnchant(command.Base.EnchantList);
        SetCost(command.CostList);
        TargetData targetData = TargetDatabase.Instance?.GetData(command.Attack.Target);
        targetImage.sprite = targetData.icon;
        ResizeDialog();
    }

    private void ResetSkillList()
    {
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
        // costList内を初期化
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
