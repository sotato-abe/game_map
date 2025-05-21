using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable
{
    [SerializeField] ConsumableBase _base;
    [SerializeField] int count;
    public ConsumableBase Base { get => _base; }
}
