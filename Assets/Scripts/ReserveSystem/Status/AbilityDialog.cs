using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDialog : VariableDialog
{
    protected override float PaddingHeight => 60f;
    public void Setup(Ability ability)
    {
        namePlate.SetName(ability.Name);
        description.SetText(ability.Description);
        ResizeDialog();
    }
}
