using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI commandName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] CostIcon costPrefab;


    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Command command)
    {
        commandName.text = command.Base.Name;
        description.text = command.Base.Description;
        SetEnchant(command.Base.EnchantList);
        SetCost(command.Base.CostList);
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        transform.gameObject.SetActive(showFlg);
    }

    private void SetEnchant(List<Enchant> enchants)
    {
        // enchantList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

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
