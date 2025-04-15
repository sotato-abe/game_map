using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hotel
{
    [SerializeField] HotelBase _base;
    public HotelBase Base => _base;

    public Hotel(HotelBase baseData)
    {
        _base = baseData;
    }
}