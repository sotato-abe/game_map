using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Cost
{
    public CostType type;
    public int val;

    public Cost(CostType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
