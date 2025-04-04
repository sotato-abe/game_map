using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerStatusDialog : Dialog
{
    [SerializeField] TextMeshProUGUI battlerName;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] GameObject enchantList;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Battler battler)
    {
        battlerName.SetText(battler.Base.Name);
        attackText.SetText(battler.Attack.val.ToString());
        techniqueText.SetText(battler.Technique.val.ToString());
        defenseText.SetText(battler.Defense.val.ToString());
        speedText.SetText(battler.Speed.val.ToString());

        ResetList();
        SetEnchant(battler.Enchants);
    }

    private void ResetList()
    {
        // skillList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetEnchant(List<Enchant> enchants)
    {
        // enchantList内にスキルを追加
        foreach (Enchant enchant in enchants)
        {
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();

            enchantUnit.SetEnchant(enchant);
        }
    }
}
