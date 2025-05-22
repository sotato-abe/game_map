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
    [SerializeField] GameObject contentList;
    [SerializeField] BattleUnit playerUnit;

    private void Start()
    {
        inventoryWindow.OnDropItemBlockAction += MoveItemBlock;
    }
    private void OnEnable()
    {
        LayoutRebuilderContent();
    }

    public void Update()
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

    public void ExecuteTurn()
    {
        OnActionExecute?.Invoke();
    }

    public void MoveItemBlock(ItemBlock item)
    {
        pouchWindow.RemoveItem(item);
    }

    private void LayoutRebuilderContent()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentList.GetComponent<RectTransform>());
    }

}
