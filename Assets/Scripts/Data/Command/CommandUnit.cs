using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : Unit
{
    public Command Command { get; set; }
    [SerializeField] Image image;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject costList;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] EnegyIcon enegyPrefab;
    [SerializeField] UnitStatusLayer unitStatusLayer;
    [SerializeField] CommandDialog dialog;


    public virtual void Setup(Command command)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
        SetEnchant();
        SetCost();
        dialog.gameObject.SetActive(true);
        dialog.Setup(Command);
        SetStatus(UnitStatus.Active);
    }

    public void OnPointerEnter()
    {
        dialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        dialog.ShowDialog(false);
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
                EnegyIcon enegyObject = Instantiate(enegyPrefab, costList.transform);
                enegyObject.gameObject.SetActive(true);
                EnegyIcon enegyUnit = enegyObject.GetComponent<EnegyIcon>();
                enegyUnit.SetCostIcon(cost);
            }
        }
    }

    public void SetStatus(UnitStatus status)
    {
        unitStatusLayer.Setup(status);
    }
}
