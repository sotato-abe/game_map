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
    [SerializeField] BattleUnit playerUnit;

    private void Start()
    {
    }
    private void OnEnable()
    {
        // LayoutRebuilderContent();
    }

    public void Update()
    {
        //BagPanelを無効化
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = false;
            OnActionExit?.Invoke();
        }
    }

    public void ExecuteTurn()
    {
        OnActionExecute?.Invoke();
    }
}
