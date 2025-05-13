using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    public void SetEnchant(Enchant enchant)
    {
        text.text = enchant.Val.ToString();
        SetEnchantIcon(enchant.Type);
    }

    private void SetEnchantIcon(EnchantType type)
    {
        EnchantData data = EnchantDatabase.Instance?.GetData(type);
        if (data != null)
        {
            image.sprite = data.icon;
        }
        else
        {
            Debug.LogWarning($"EnchantIcon: No data found for type {type}");
        }
    }
}

