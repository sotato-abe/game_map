using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Enchant
{
    [SerializeField] EnchantType type;
    [SerializeField] int val;

    public EnchantType Type { get => type; }
    public int Val { get => val; }

    public Enchant(EnchantType type, int val)
    {
        this.type = type;
        this.val = val;
    }

}
