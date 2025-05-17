using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantDialog : Dialog
{
    private int dialogWidth = 300;
    private int padding = 60;
    public void Setup(Enchant enchant)
    {
        EnchantData data = EnchantDatabase.Instance?.GetData(enchant.Type);
        namePlate.SetName(data.enchantName);
        description.SetText(data.description);
        Invoke(nameof(ResizePlate), 0f);
    }

    private void ResizePlate()
    {
        float newHeight = description.preferredHeight + padding;
        GetComponent<RectTransform>().sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
