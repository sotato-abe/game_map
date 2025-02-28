using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : MonoBehaviour
{
    public Command Command { get; set; }
    [SerializeField] Image image;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] CostIcon costPrefab;
    [SerializeField] CommandDialog commandDialog;

    public virtual void Setup(Command command)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
        SetEnchant();
        SetCost();
        commandDialog.Setup(Command);
    }

    public void OnPointerEnter()
    {
        // StartCoroutine(Targetfoucs(true));
        commandDialog.ShowDialog(true);
    }

    public void OnPointerExit()
    {
        // StartCoroutine(Targetfoucs(false));
        commandDialog.ShowDialog(false);
    }

    private void SetEnchant()
    {
        // enchantList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

        // enchantList内にスキルを追加
        foreach (var enchant in Command.Base.EnchantList)
        {
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();

            enchantUnit.SetEnchant(enchant);
        }
    }

    private void SetCost()
    {
        // costList内のオブジェクトを削除
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // costList内にコストを追加
        foreach (var cost in Command.Base.CostList)
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
