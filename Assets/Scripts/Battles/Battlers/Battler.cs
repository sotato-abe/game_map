using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler 
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;

    public BattlerBase Base { get => _base; }
    public int Level { get => level; }

    public int MaxHP { get; set;}
    public int HP { get; set;}
    public int Attack { get; set;}

    public void Init()
    {
        MaxHP = _base.MaxHP ;
        HP = MaxHP ;
        Attack = _base.Attack ;
    }
}
