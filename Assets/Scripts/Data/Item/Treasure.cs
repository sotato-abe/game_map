using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Treasure
{
    [SerializeField] TreasureBase _base;
    [SerializeField] int count;
    public TreasureBase Base { get => _base; }
}
