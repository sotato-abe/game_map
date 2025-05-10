using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDialog : Dialog
{
    int dialogWidth = 300;
    int padding = 60;
    public void Setup(Ability ability)
    {
        namePlate.SetName(ability.Name);
        description.SetText(ability.Description);
        Invoke(nameof(ResizePlate), 0f);
    }

    private void ResizePlate()
    {
        float newHeight = description.preferredHeight + padding;
        GetComponent<RectTransform>().sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
