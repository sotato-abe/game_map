using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;
    [SerializeField] int soul;

    public BattlerBase Base { get => _base; }
    public int Level { get => level; }
    public int Soul { get => soul; set => soul = value; }
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

    public List<Equipment> Equipments { get; set; }

    public int MaxInventoryCount { get; set; }
    public List<Item> Inventory { get; set; }
    public List<Command> Deck { get; set; }
    public Coordinate coordinate;

    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("Init() failed: _base is null");
            return;
        }

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
        Equipments = new List<Equipment>(_base.Equipments ?? new List<Equipment>());
        MaxInventoryCount = _base.MaxInventoryCount;
        Inventory = new List<Item>(_base.Inventory ?? new List<Item>());
        Deck = new List<Command>(_base.Commands ?? new List<Command>());

        if (_base.Birthplace != null)
            coordinate = _base.Birthplace.Coordinate;
    }

    public void TakeDamage(List<Damage> damageList)
    {
        foreach (Damage damage in damageList)
        {
            Debug.Log($"damage:{damage.AttackType} /{damage.Val}");
            if (damage.AttackType == AttackType.Enegy)
            {
                if (damage.SubType == (int)EnegyType.Life)
                {
                    Life = Life - damage.Val;
                }
                if (damage.SubType == (int)EnegyType.Battery)
                {
                    Battery = Battery - damage.Val;
                }
                if (damage.SubType == (int)EnegyType.Soul)
                {
                    Soul = Soul - damage.Val;
                }
            }
        }
        Debug.Log($"Life/{Life}");
        Debug.Log($"Battery/{Battery}");
        Debug.Log($"Soul/{Soul}");
    }

    public bool AddItemToInventory(Item item)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        if (Inventory.Count >= MaxInventoryCount)
        {
            Debug.Log("Inventory is full.");
            return false;
        }
        Inventory.Add(item); // inventory はプレイヤーのインベントリリスト
        return true;
    }

    public void AddEquipmentToEquipments(Equipment equipment)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        Equipments.Add(equipment); // inventory はプレイヤーのインベントリリスト
    }

    public void AddCommandToDeck(Command command)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        Deck.Add(command); // inventory はプレイヤーのインベントリリスト
    }
}
