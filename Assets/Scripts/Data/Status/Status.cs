using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Status
{
    public StatusType type;
    public int val;

    public Status(StatusType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
