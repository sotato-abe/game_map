using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnchantCount
{
    [SerializeField] EnchantType type;
    [SerializeField] TargetType target;
    [SerializeField] int val;

    public EnchantType Type { get => type; }
    public TargetType Target { get => target; }
    public int Val { get => val; set => val = value; }  // setter を追加

    public EnchantCount(EnchantType type, TargetType target, int val)
    {
        this.type = type;
        this.target = target;
        this.val = val;
    }

}
