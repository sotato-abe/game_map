using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Cost
{
    public EnegyType type;
    public int val;

    public Cost(EnegyType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
