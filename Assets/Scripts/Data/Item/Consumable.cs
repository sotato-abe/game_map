using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    [SerializeField] ConsumableBase _base;
    [SerializeField] int count = 1;

    public override ItemType itemType => ItemType.Consumable;
    public override ItemBase Base => _base;
    public ConsumableBase ConsumableBase { get => _base; }

    public int Count => count;

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
