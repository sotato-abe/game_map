using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;
    [SerializeField] int soul;
    [SerializeField] private List<Equipment> equipments = new List<Equipment>();
    [SerializeField] private List<Item> inventory = new List<Item>();
    [SerializeField] private List<Command> deck = new List<Command>();

    public List<Equipment> Equipments { get => equipments; }
    public List<Item> Inventory { get => inventory; }
    public List<Command> Deck { get => deck; }
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
    public int Money { get; set; }
    public int Disk { get; set; }
    public int Key { get; set; }
    private Coordinate currentField;

    public virtual void Init()
    {
        MaxLife = _base.MaxLife;
        Life = MaxLife;
        MaxBattery = _base.MaxBattery;
        Battery = MaxBattery;
        Attack = _base.Attack;
        Technique = _base.Technique;
        Defense = _base.Defense;
        Speed = _base.Speed;
        Money = _base.Money;
        Disk = _base.Disk;
        Key = _base.Key;

        equipments = _base.Equipments ?? new List<Equipment>(); // Items が null の場合、新しいリストを初期化
        inventory = _base.Items ?? new List<Item>(); // Items が null の場合、新しいリストを初期化
        deck = _base.Commands ?? new List<Command>(); // Items が null の場合、新しいリストを初期化
    }

    public Coordinate GetBirthCoordinate()
    {
        return new Coordinate
        {
            row = _base.Birthplace.Coordinate.row,
            col = _base.Birthplace.Coordinate.col,
        };
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

    public void AddCommandToDeck(Command command)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        deck.Add(command); // inventory はプレイヤーのインベントリリスト
    }
}
