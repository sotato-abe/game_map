using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI equipmentName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] EnchantIcon enchantIcon;
    [SerializeField] EnegyIcon costPrefab;


    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Equipment equipment)
    {
        equipmentName.text = equipment.Base.Name;
        description.text = equipment.Base.Description;
        ResetSkillList();
        SetAttack(equipment.Base.AttackList);
        SetEnchant(equipment.Base.EnchantList);
        SetCost(equipment.Base.CostList);
    }

    private void ResetSkillList()
    {
        // skillList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetAttack(List<Enegy> attacks)
    {
        // attackList内に攻撃力を追加
        foreach (var attack in attacks)
        {
            EnegyIcon attackObject = Instantiate(enegyPrefab, enchantList.transform);
            attackObject.gameObject.SetActive(true);
            EnegyIcon attackUnit = attackObject.GetComponent<EnegyIcon>();
            attackUnit.SetCostIcon(attack);
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
