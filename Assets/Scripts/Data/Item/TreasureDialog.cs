using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureDialog : VariableDialog
{
    protected override float PaddingHeight => 60f;
    public void Setup(Item item)
    {
        if (item is Treasure treasure)
        {
            namePlate.SetName(item.Base.Name);
            description.text = item.Base.Description;
            ResizeDialog();
        }
    }
}
