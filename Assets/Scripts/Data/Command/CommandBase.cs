using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CommandBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] CommandType commandType;
    [SerializeField] EnchantType enchantType; //コマンドがエンチャントのときにタイプとして使用する
    [SerializeField] int value; // 攻撃の実数
    [SerializeField] int count; // 攻撃や回復の回数、エンチャントの個数に使用する
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;
    [SerializeField] Sprite sprite;
    [SerializeField] string description;
    [SerializeField] private List<Enchant> enchantList = new List<Enchant>();

    public string Name { get => name; }
    public CommandType CommandType { get => commandType; }
    public EnchantType EnchantType { get => enchantType; }
    public int Value { get => value; }
    public int Count { get => count; }
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
    public List<Enchant> EnchantList { get => enchantList; }
}
