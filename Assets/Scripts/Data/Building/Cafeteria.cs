using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cafeteria
{
    [SerializeField] CafeteriaBase _base;
    public CafeteriaBase Base => _base;

    public List<Item> Items { get; private set; }

    public Cafeteria(CafeteriaBase baseData)
    {
        _base = baseData;
        Items = new List<Item>(_base?.Items ?? new List<Item>());
    }
}