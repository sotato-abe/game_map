using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagPanel : Panel
{
    [SerializeField] BagCategoryIcon categoryPrefab;
    [SerializeField] InventoryDialog inventoryDialog;
    [SerializeField] PouchWindow pouchWindow;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] ImplantWindow implantWindow;
    [SerializeField] GameObject categoryList;
    [SerializeField] BattleUnit playerUnit;

    private List<BagCategoryIcon> categoryIconList = new List<BagCategoryIcon>();

    BagCategory selectedCategory = BagCategory.All; // TODO：bagCategoryに変更

    private void Start()
    {
        pouchWindow.gameObject.SetActive(false);
        equipmentWindow.gameObject.SetActive(false);
        implantWindow.gameObject.SetActive(false);
        SetCategoryList();
    }
    private void SetCategoryList()
    {
        // 一度CategoryListの中身を空の状態にする
        foreach (Transform child in categoryList.transform)
        {
            Destroy(child.gameObject);
        }
        categoryIconList.Clear();

        foreach (BagCategory categoryValue in System.Enum.GetValues(typeof(BagCategory)))
        {
            BagCategoryIcon categoryIcon = Instantiate(categoryPrefab, categoryList.transform);
            categoryIconList.Add(categoryIcon);
            categoryIcon.SetCategory(categoryValue);
            if (selectedCategory == categoryValue)
            {
                categoryIcon.SetActive(true);
            }
        }
    }

    private void ChangeCategory()
    {
        foreach (BagCategoryIcon icon in categoryIconList)
        {
            icon.SetActive(false);
        }
        categoryIconList[(int)selectedCategory].SetActive(true);
    }

    public void SelectDialog(bool selectDirection)
    {
        BagCategory newselectedCategory = selectedCategory;
        if (selectDirection)
        {
            newselectedCategory++;
            if (newselectedCategory > BagCategory.Tresure)
            {
                newselectedCategory = BagCategory.All;
            }
        }
        else
        {
            newselectedCategory--;
            if (newselectedCategory < BagCategory.All)
            {
                newselectedCategory = BagCategory.Tresure;
            }
        }
        ChangeDialog(newselectedCategory);
        ChangeCategory();
    }

    public void ChangeDialog(BagCategory dialog)
    {
        if (selectedCategory == dialog)
        {
            return;
        }

        ResetDialog();
        switch (dialog)
        {
            case BagCategory.All:
                // TODO：インベントリのステータスを表示する
                break;
            case BagCategory.Pouch:
                pouchWindow.gameObject.SetActive(true);
                break;
            case BagCategory.Equip:
                equipmentWindow.gameObject.SetActive(true);
                break;
            case BagCategory.Implant:
                implantWindow.gameObject.SetActive(true);
                break;
            case BagCategory.Tresure:
                break;
        }
        selectedCategory = dialog;
    }

    private void ResetDialog()
    {
        pouchWindow.gameObject.SetActive(false);
        equipmentWindow.gameObject.SetActive(false);
        implantWindow.gameObject.SetActive(false);
    }

    private void SetItemUnit()
    {
        inventoryDialog.SetItemUnit(playerUnit.Battler.Inventory);
    }
}
