using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureDialog : VariableDialog
{
    public void Setup(Item item)
    {
        if (item is Consumable consumable)
        {
            namePlate.SetName(item.Base.Name);
            description.text = item.Base.Description;
        }
    }
}
