using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kiosk
{
    [SerializeField] KioskBase _base;
    public KioskBase Base => _base;

    public List<Item> Items { get; private set; }

    public Kiosk(KioskBase baseData)
    {
        _base = baseData;
        Items = new List<Item>(_base?.Items ?? new List<Item>());
    }
}