using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmsShop
{
    [SerializeField] ArmsShopBase _base;
    public ArmsShopBase Base => _base;

    public List<Equipment> Equipments { get; private set; }

    public ArmsShop(ArmsShopBase baseData)
    {
        _base = baseData;
        Equipments = new List<Equipment>(_base?.Equipments ?? new List<Equipment>());
    }
}