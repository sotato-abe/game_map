using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] ItemType itemType;
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;
    // [SerializeField] int life;
    // [SerializeField] int battery;
    // [SerializeField] int soul;
    [SerializeField] Sprite sprite;
    [SerializeField] string description;
    [SerializeField] private List<Enegy> recoveryList = new List<Enegy>();
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public string Name { get => name; }
    public Enegy LifeCost => new Enegy(EnegyType.Life, lifeCost);
    public Enegy BatteryCost => new Enegy(EnegyType.Battery, batteryCost);
    public Enegy SoulCost => new Enegy(EnegyType.Soul, soulCost);
    public List<Enegy> CostList => new List<Enegy>
    {
        LifeCost,
        BatteryCost,
        SoulCost,
    };
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public List<Enegy> RecoveryList { get => recoveryList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
