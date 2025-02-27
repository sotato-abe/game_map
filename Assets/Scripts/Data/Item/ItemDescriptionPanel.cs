using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] CostIcon enegyPrefab;
    [SerializeField] CostIcon costPrefab;
    [SerializeField] EnchantIcon enchantPrefab;

    void Start()
    {
        transform.gameObject.SetActive(false);
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

    public void ShowDescriptionPanel(bool showFlg)
    {
        Debug.Log($"ItemDescriptionPanel ShowDescriptionPanel:{showFlg}");
        transform.gameObject.SetActive(showFlg);
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
            CostIcon enegyObject = Instantiate(enegyPrefab, enchantList.transform);
            enegyObject.gameObject.SetActive(true);
            CostIcon enegyUnit = enegyObject.GetComponent<CostIcon>();
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
                CostIcon costObject = Instantiate(costPrefab, costList.transform);
                costObject.gameObject.SetActive(true);
                CostIcon costUnit = costObject.GetComponent<CostIcon>();
                costUnit.SetCostIcon(cost);
            }
        }
    }
}
