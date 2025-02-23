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
    [SerializeField] Sprite sprite;
    [SerializeField] string description;

    public string Name { get => name; }
    public CommandType CommandType { get => commandType; }
    public EnchantType EnchantType { get => enchantType; }
    public int Value { get => value; }
    public int Count { get => count; }
    public int LifeCost { get => lifeCost; }
    public int BatteryCost { get => batteryCost; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
}
