using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [SerializeField] EquipmentBase _base;

    public EquipmentBase Base { get => _base; }

    public int Life { get; set; }
    public int Battery { get; set; }
    public int Attack { get; set; }
    public int Technique { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }

    public void Init()
    {
        Life = Life;
        Battery = Battery;
        Attack = Attack;
        Technique = Technique;
        Defense = Defense;
        Speed = Speed;
    }
}
