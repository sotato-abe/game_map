using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [SerializeField] EquipmentBase _base;
    [SerializeField] int Level = 1;
    
    public EquipmentBase Base { get => _base; }
}
