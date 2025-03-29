using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] EnchantIconList enchantIconList;

    public void SetEnchant(Enchant enchant)
    {
        text.text = enchant.Val.ToString();
        image.sprite = enchantIconList.GetIcon(enchant.Type);
    }
}

