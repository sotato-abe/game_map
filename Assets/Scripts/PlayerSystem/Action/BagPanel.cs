using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagPanel : Panel
{
    [SerializeField] InventoryDialog inventoryDialog;
    [SerializeField] PouchDialog pouchDialog;
    [SerializeField] EquipmentDialog equipmentDialog;
    [SerializeField] ImplantDialog implantDialog;
    [SerializeField] BattleUnit playerUnit;

    int targetDialog;

    private void Start()
    {
        pouchDialog.gameObject.SetActive(false);
        equipmentDialog.gameObject.SetActive(false);
        implantDialog.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
    }

    public void UseItem()
    {
    }
}
