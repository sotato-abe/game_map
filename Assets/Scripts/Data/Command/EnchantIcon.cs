using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    public void SetEnchantIcon(Enchant enchant)
    {
        text.text = enchant.Val.ToString();
    }
}

