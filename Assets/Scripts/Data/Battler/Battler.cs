using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO BattlerのライフとかをEnegyに変換
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
    public int MaxPouchCount { get; set; }
    public int MaxInventoryCount { get; set; }
    public int MaxStorageCount { get; set; }
    public List<Equipment> Equipments { get; set; }
    public List<Item> Pouch { get; set; }
    public List<Item> Inventory { get; set; }
    public List<Command> RunTable { get; set; }
    public List<Command> Deck { get; set; }
    public List<Command> Storage { get; set; }
    public List<Enchant> Enchants = new List<Enchant>();
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
        MaxPouchCount = _base.MaxPouchCount;
        MaxInventoryCount = _base.MaxInventoryCount;
        MaxStorageCount = _base.MaxStorageCount;
        Equipments = new List<Equipment>(_base.Equipments ?? new List<Equipment>());
        Pouch = new List<Item>(_base.Pouch ?? new List<Item>());
        Inventory = new List<Item>(_base.Inventory ?? new List<Item>());
        RunTable = new List<Command>(_base.RunTable ?? new List<Command>());
        Deck = new List<Command>();
        Enchants = new List<Enchant>();
        Storage = new List<Command>();

        if (_base.Birthplace != null)
            coordinate = _base.Birthplace.Coordinate;
    }

    public void TakeRecovery(List<Enegy> recoveryList)
    {
        foreach (Enegy recovery in recoveryList)
        {
            if (recovery.type == EnegyType.Life)
            {
                Life = Mathf.Min(Life + recovery.val, MaxLife);
            }
            if (recovery.type == EnegyType.Battery)
            {
                Battery = Mathf.Min(Battery + recovery.val, MaxBattery);
            }
            if (recovery.type == EnegyType.Soul)
            {
                Soul = Soul + recovery.val;
            }
        }
    }

    // ライフを割り切るときにfalseを返す（isAlive）
    public void TakeDamage(List<Damage> damageList)
    {
        foreach (Damage damage in damageList)
        {
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
    }

    public void TakeEnchant(List<Enchant> enchantList)
    {
        foreach (Enchant enchant in enchantList)
        {
            Enchant existingEnchant = Enchants.Find(e => e.Type == enchant.Type);
            if (existingEnchant != null)
            {
                // 既存のEnchantの値を加算
                existingEnchant.Val += enchant.Val;
            }
            else
            {
                // 新規追加
                Enchants.Add(new Enchant(enchant.Type, enchant.Val));
            }
        }
    }


    public bool AddItemToPouch(Item item)
    {
        // プレイヤーのポーチにアイテムを追加する処理
        if (Pouch.Count >= MaxPouchCount)
        {
            Debug.Log("Pouch is full.");
            return false;
        }
        Pouch.Add(item);
        return true;
    }

    public bool AddItemToInventory(Item item)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        if (Inventory.Count >= MaxInventoryCount)
        {
            Debug.Log("Inventory is full.");
            return false;
        }
        Inventory.Add(item);
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
