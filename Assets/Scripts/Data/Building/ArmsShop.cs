using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmsShop
{
    [SerializeField] ArmsShopBase _base;
    public ArmsShopBase Base => _base;

    public ArmsShop(ArmsShopBase baseData)
    {
        _base = baseData;
    }
}