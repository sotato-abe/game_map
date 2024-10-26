using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;
    [SerializeField] int soul;

    [SerializeField] private List<Item> inventory = new List<Item>();
    public List<Item> Inventory { get => inventory; }

    public BattlerBase Base { get => _base; }
    public int Level { get => level; }
    public int Soul { get => soul; }

    public int MaxLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int Battery { get; set; }
    public int Attack { get; set; }
    public int Technique { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }

    public void Init()
    {
        MaxLife = _base.MaxLife;
        Life = MaxLife;
        MaxBattery = _base.MaxBattery;
        Battery = MaxBattery;
        Attack = _base.Attack;
        Technique = _base.Technique;
        Defense = _base.Defense;
        Speed = _base.Speed;

        inventory = _base.Items ?? new List<Item>(); // Items が null の場合、新しいリストを初期化
    }

public int TakeDamage(Battler attacker)
{
    int damage = attacker.Attack;
    Life = Mathf.Clamp(Life - damage, 0, MaxLife);
    return damage;
}

    public void AddItemToInventory(Item item)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        inventory.Add(item); // inventory はプレイヤーのインベントリリスト
    }
}
