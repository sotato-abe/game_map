using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] ItemType itemType;
    [SerializeField] Sprite sprite;
    [SerializeField, TextArea] string description;
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;
    [SerializeField] private List<Enegy> recoveryList = new List<Enegy>();
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public ItemType ItemType { get => itemType; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public Enegy LifeCost => new Enegy(EnegyType.Life, lifeCost);
    public Enegy BatteryCost => new Enegy(EnegyType.Battery, batteryCost);
    public Enegy SoulCost => new Enegy(EnegyType.Soul, soulCost);
    public List<Enegy> CostList => new List<Enegy>
    {
        LifeCost,
        BatteryCost,
        SoulCost,
    };
    public List<Enegy> RecoveryList { get => recoveryList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
