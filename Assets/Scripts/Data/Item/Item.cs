using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] ItemBase _base;

    public ItemBase Base { get => _base; }

    public int Life { get; set; }
    public int Battery { get; set; }

    public void Init()
    {
        Life = Life;
        Battery = Battery;
    }
}
