using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : Item
{
    [SerializeField] EquipmentBase _base;
    [SerializeField] int level = 1;

    public override ItemType itemType => ItemType.Equipment;
    public override ItemBase Base => _base; // EquipmentBase は ItemBase を継承している前提
    public EquipmentBase EquipmentBase => _base;
    public int Level => level;

    public Attack Attack => new Attack(
        _base.TargetType,
        new List<Enegy>(_base.DamageList),
        new List<Enegy>(_base.RecoveryList),
        new List<Enchant>(_base.EnchantList)
    );
    public List<Enegy> EnegyList => new List<Enegy>
    {
        new Enegy(EnegyType.Life, _base.Life),
        new Enegy(EnegyType.Battery, _base.Battery)
    };

    public List<Status> StatusList => new List<Status>
    {
        new Status(StatusType.POW, _base.Power),
        new Status(StatusType.TEC, _base.Technique),
        new Status(StatusType.DEF, _base.Defense),
        new Status(StatusType.SPD, _base.Speed),
        new Status(StatusType.LUK, _base.Luck),
        new Status(StatusType.MMR, _base.Memory),
        new Status(StatusType.STG, _base.Storage),
        new Status(StatusType.POC, _base.Pouch),
        new Status(StatusType.BAG, _base.Bag)
    };
}
