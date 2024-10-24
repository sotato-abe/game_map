using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;
    [SerializeField] int soul;

    public BattlerBase Base { get => _base; }
    public int Level { get => level; }
    public int Soul { get => soul; }

    public int MaxLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int Battery { get; set; }
    public int Attack { get; set; }
    public int Technique { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }

    public void Init()
    {
        MaxLife = _base.MaxLife;
        Life = MaxLife;
        MaxBattery = _base.MaxBattery;
        Battery = MaxBattery;
        Attack = _base.Attack;
        Technique = _base.Technique;
        Defense = _base.Defense;
        Speed = _base.Speed;
    }

    public int TakeDamege(Battler attacker)
    {
        int damage = attacker.Attack;

        Life = Mathf.Clamp(Life - damage, 0, MaxLife);
        return damage;
    }
}
