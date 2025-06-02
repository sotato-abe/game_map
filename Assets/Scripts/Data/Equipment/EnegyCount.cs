using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnegyCount
{
    public EnegyType type;
    public int val;
    public int max;
    public int cal;

    public EnegyCount(EnegyType type, int val, int max, int cal)
    {
        this.type = type;
        this.val = val;
        this.max = max;
        this.cal = cal;
    }
}
