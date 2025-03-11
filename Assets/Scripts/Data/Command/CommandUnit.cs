using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : Unit
{
    public Command command { get; set; }
    [SerializeField] Image image;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] CommandDialog commandDialog;

    public virtual void Setup(Command command)
    {
        this.command = command;
        image.sprite = this.command.Base.Sprite;
        SetEnchant();
        SetCost();
        commandDialog.Setup(this.command);
    }

    public void OnPointerEnter()
    {
        commandDialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        commandDialog.ShowDialog(false);
        StartCoroutine(OnPointer(false));
    }

    private void SetEnchant()
    {
        // enchantList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

        // enchantList内にスキルを追加
        foreach (var enchant in command.Base.EnchantList)
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
        foreach (var cost in command.Base.CostList)
        {
            if (0 < cost.val)
            {
                EnegyIcon enegyObject = Instantiate(enegyPrefab, costList.transform);
                enegyObject.gameObject.SetActive(true);
                EnegyIcon enegyUnit = enegyObject.GetComponent<EnegyIcon>();
                enegyUnit.SetCostIcon(cost);
            }
        }
    }
}
