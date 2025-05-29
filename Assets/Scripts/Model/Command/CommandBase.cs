using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Command/CommandData")]
public class CommandBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] Sprite sprite;
    [SerializeField, TextArea] string description;
    [SerializeField] int capacity;

    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;

    [SerializeField] TargetType targetType;
    [SerializeField] private List<Enegy> damageList = new List<Enegy>();
    [SerializeField] private List<Enegy> recoveryList = new List<Enegy>();
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
    public int Capacity { get => capacity; }


    public Enegy LifeCost => new Enegy(EnegyType.Life, lifeCost);
    public Enegy BatteryCost => new Enegy(EnegyType.Battery, batteryCost);
    public Enegy SoulCost => new Enegy(EnegyType.Soul, soulCost);

    public TargetType TargetType { get => targetType; }
    public List<Enegy> DamageList { get => damageList; }
    public List<Enegy> RecoveryList { get => recoveryList; }
    public List<Enchant> EnchantList { get => enchantList; }
}
