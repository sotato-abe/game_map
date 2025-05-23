using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentBase", menuName = "Item/EquipmentBase")]
public class EquipmentBase : ItemBase
{
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int power;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int luck;

    [SerializeField] int probability = 100;
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;

    [SerializeField] TargetType targetType;
    [SerializeField] private List<Enegy> damageList = new List<Enegy>();
    [SerializeField] private List<Enegy> recoveryList = new List<Enegy>();
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public EquipmentType EquipmentType { get => equipmentType; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Power { get => power; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public int Luck { get => luck; }

    public Probability Probability { get => probability; }
    public Enegy LifeCost => new Enegy(EnegyType.Life, lifeCost);
    public Enegy BatteryCost => new Enegy(EnegyType.Battery, batteryCost);
    public Enegy SoulCost => new Enegy(EnegyType.Soul, soulCost);
    public List<Enegy> CostList => new List<Enegy> { LifeCost, BatteryCost, SoulCost, };
    
    public TargetType TargetType { get => targetType; }
    public List<Enegy> DamageList { get => damageList; }
    public List<Enegy> RecoveryList { get => recoveryList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
