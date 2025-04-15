using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Laboratory
{
    [SerializeField] LaboratoryBase _base;
    public LaboratoryBase Base => _base;

    public Laboratory(LaboratoryBase baseData)
    {
        _base = baseData;
    }
}