using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] EquipmentType type;
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int attack;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;
    [SerializeField] Probability probability;
    [SerializeField] string description;
    [SerializeField] private List<Enegy> attackList = new List<Enegy>();
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public string Name { get => name; }
    public EquipmentType Type { get => type; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public Enegy LifeCost => new Enegy(EnegyType.Life, lifeCost);
    public Enegy BatteryCost => new Enegy(EnegyType.Battery, batteryCost);
    public Enegy SoulCost => new Enegy(EnegyType.Soul, soulCost);
    public Probability Probability { get => probability; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public List<Enegy> AttackList { get => attackList; }
    public List<Enchant> EnchantList { get => enchantList; }
    public List<Enegy> CostList => new List<Enegy>
    {
        LifeCost,
        BatteryCost,
        SoulCost,
    };
}
