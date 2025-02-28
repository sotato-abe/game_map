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
        attackText.SetText(battler.Attack.ToString());
        techniqueText.SetText(battler.Technique.ToString());
        defenseText.SetText(battler.Defense.ToString());
        speedText.SetText(battler.Speed.ToString());

        ResetList();
        SetEnchant(battler.Enchants);
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        Debug.Log($"Show BattlerStatusPanel:{showFlg}");
        transform.gameObject.SetActive(showFlg);
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
