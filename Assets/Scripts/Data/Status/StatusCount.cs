using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class StatusCount
{
    public StatusType type;
    public int val;
    public int cal;

    public StatusCount(StatusType type, int val, int cal)
    {
        this.type = type;
        this.val = val;
        this.cal = cal;
    }
}
