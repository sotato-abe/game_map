using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnegyIcon costPrefab;
    [SerializeField] EnchantIcon enchantPrefab;

    void Start()
    {
        ShowDialog(false);
    }

    public virtual void Setup(Item item)
    {
        itemName.text = item.Base.Name;
        description.text = item.Base.Description;
        ResetListList();
        SetEnegy(item.Base.RecoveryList);
        SetEnchant(item.Base.EnchantList);
        SetCost(item.Base.CostList);
    }

    private void ResetListList()
    {
        // skillList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetEnegy(List<Enegy> enegys)
    {
        // AttackList内にエネルギーを追加
        foreach (var enegy in enegys)
        {
            EnegyIcon enegyObject = Instantiate(enegyPrefab, enchantList.transform);
            enegyObject.gameObject.SetActive(true);
            EnegyIcon enegyUnit = enegyObject.GetComponent<EnegyIcon>();
            enegyUnit.SetCostIcon(enegy);
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
