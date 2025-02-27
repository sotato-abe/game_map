using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagPanel : Panel
{
    [SerializeField] BagCategoryIcon categoryPrefab;
    [SerializeField] InventoryDialog inventoryDialog;
    [SerializeField] PouchDialog pouchDialog;
    [SerializeField] EquipmentDialog equipmentDialog;
    [SerializeField] ImplantDialog implantDialog;
    [SerializeField] GameObject categoryList;
    [SerializeField] BattleUnit playerUnit;

    private List<BagCategoryIcon> categoryIconList = new List<BagCategoryIcon>();

    BagCategory selectedCategory = BagCategory.All; // TODO：bagCategoryに変更

    private void Start()
    {
        pouchDialog.gameObject.SetActive(false);
        equipmentDialog.gameObject.SetActive(false);
        implantDialog.gameObject.SetActive(false);
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
        Debug.Log($"{(int)selectedCategory}:{categoryIconList.Count}");
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
                pouchDialog.gameObject.SetActive(true);
                break;
            case BagCategory.Equipment:
                equipmentDialog.gameObject.SetActive(true);
                break;
            case BagCategory.Implant:
                implantDialog.gameObject.SetActive(true);
                break;
            case BagCategory.Tresure:
                break;
        }
        selectedCategory = dialog;
    }

    private void ResetDialog()
    {
        pouchDialog.gameObject.SetActive(false);
        equipmentDialog.gameObject.SetActive(false);
        implantDialog.gameObject.SetActive(false);
    }

    private void SetItemUnit()
    {
        inventoryDialog.SetItemUnit(playerUnit.Battler.Inventory);
    }
}
