using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Command
{
    [SerializeField] CommandBase _base;

    public CommandBase Base { get => _base; }

    public List<Enegy> CostList => new List<Enegy>
    {
        _base.LifeCost,
        _base.BatteryCost,
        _base.SoulCost,
    };

    public Attack Attack => new Attack(
        _base.TargetType,
        new List<Enegy>(_base.DamageList),
        new List<Enegy>(_base.RecoveryList),
        new List<Enchant>(_base.EnchantList)
    );
}
