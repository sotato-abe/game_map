using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Enegy
{
    public EnegyType type;
    public int val;

    public Enegy(EnegyType type, int val)
    {
        this.type = type;
        this.val = val;
    }
}
