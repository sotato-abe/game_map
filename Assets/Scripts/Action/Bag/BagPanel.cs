using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagPanel : Panel
{
    [SerializeField] BagCategoryIcon categoryPrefab;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] PouchWindow pouchWindow;
    [SerializeField] EquipmentWindow equipmentWindow;
    [SerializeField] GameObject categoryList;
    [SerializeField] BattleUnit playerUnit;

    private List<BagCategoryIcon> categoryIconList = new List<BagCategoryIcon>();
    private BagCategory selectedCategory = BagCategory.Pouch;

    private void Start()
    {
        ResetDialog();
        inventoryWindow.OnDropItemUnitAction += MoveItemUnit;
        SetCategoryList();
    }
    private void OnEnable()
    {
    }

    public void Update()
    {

        if (!isActive)
        {
            //BagCategoryを変更
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectWindow(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectWindow(false);
            }
            //BagPanelを有効化
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isActive = true;
            }
        }
        else
        {
            //BagPanelを無効化
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isActive = false;
                OnActionExit?.Invoke();
            }

            //InventoryWindowを操作
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inventoryWindow.SelectItem(ArrowType.Up);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inventoryWindow.SelectItem(ArrowType.Right);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inventoryWindow.SelectItem(ArrowType.Down);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inventoryWindow.SelectItem(ArrowType.Left);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inventoryWindow.UseItem();
            }
        }
    }

    public void ExecuteTurn()
    {
        OnActionExecute?.Invoke();
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
        ChangeCategory();
    }

    public void SelectWindow(bool selectDirection)
    {
        BagCategory newselectedCategory = selectedCategory;
        if (selectDirection)
        {
            newselectedCategory++;
            if (newselectedCategory > BagCategory.Tresure)
            {
                newselectedCategory = BagCategory.Pouch;
            }
        }
        else
        {
            newselectedCategory--;
            if (newselectedCategory < BagCategory.Pouch)
            {
                newselectedCategory = BagCategory.Tresure;
            }
        }
        selectedCategory = newselectedCategory;
        ChangeCategory();
    }

    public void ChangeCategory()
    {
        foreach (BagCategoryIcon icon in categoryIconList)
        {
            icon.SetActive(false);
        }
        categoryIconList[(int)selectedCategory].SetActive(true);


        ResetDialog();
        switch (selectedCategory)
        {
            case BagCategory.Pouch:
                pouchWindow.gameObject.SetActive(true);
                break;
            case BagCategory.Equip:
                equipmentWindow.gameObject.SetActive(true);
                break;
            case BagCategory.Tresure:
                break;
        }
    }

    private void ResetDialog()
    {
        pouchWindow.gameObject.SetActive(false);
        equipmentWindow.gameObject.SetActive(false);
    }

    public void MoveItemUnit(ItemUnit itemUnit)
    {
        switch (selectedCategory)
        {
            case BagCategory.Pouch:
                pouchWindow.RemoveItem(itemUnit);
                break;
            case BagCategory.Equip:
                break;
            case BagCategory.Tresure:
                break;
        }
    }
}
