using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO BattlerのライフとかをEnegyに変換
[System.Serializable]
public class Battler
{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level = 1;
    [SerializeField] int soul = 0;

    public BattlerBase Base { get => _base; }
    public int Level { get => level; set => level = value; }
    public int Soul { get => soul; set => soul = value; }
    public int MaxLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int Battery { get; set; }

    public Status Attack { get; set; }
    public Status Technique { get; set; }
    public Status Defense { get; set; }
    public Status Speed { get; set; }
    public Status Luck { get; set; }
    public Status Memory { get; set; }
    public Status Storage { get; set; }
    public Status Pouch { get; set; }
    public Status Bag { get; set; }
    public int Money { get; set; }
    public int Disk { get; set; }
    public int Key { get; set; }
    public int Exp { get; set; }

    public List<Ability> AbilityList = new List<Ability>();
    public List<Equipment> EquipmentList { get; set; }
    public List<Command> RunTable { get; set; }
    public List<Command> DeckList { get; set; }
    public List<Command> StorageList { get; set; }
    public List<Consumable> PouchList { get; set; }
    public List<Item> BagItemList { get; set; }
    // public List<Consumable> BagConsumableList { get; set; }
    // public List<Equipment> BagEquipmentList { get; set; }
    // public List<Treasure> BagTreasureList { get; set; }
    public List<Enchant> Enchants = new List<Enchant>();
    public Vector2Int coordinate;

    public List<Status> StatusList => new List<Status>
    {
        Attack,
        Technique,
        Defense,
        Speed,
        Luck,
        Memory,
        Storage,
        Pouch,
        Bag,
    };

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
        Luck = _base.Luck;
        Memory = _base.Memory;
        Storage = _base.Storage;
        Pouch = _base.Pouch;
        Bag = _base.Bag;
        Money = _base.Money;
        Disk = _base.Disk;
        Key = _base.Key;
        Exp = _base.Exp;

        AbilityList = new List<Ability>(_base.AbilityList ?? new List<Ability>());
        EquipmentList = new List<Equipment>(_base.EquipmentList ?? new List<Equipment>());
        PouchList = new List<Consumable>(_base.PouchList ?? new List<Consumable>());
        // BagConsumableList = new List<Consumable>(_base.BagConsumableList ?? new List<Consumable>());
        // BagEquipmentList = new List<Equipment>(_base.BagEquipmentList ?? new List<Equipment>());
        // BagTreasureList = new List<Treasure>(_base.BagTreasureList ?? new List<Treasure>());
        BagItemList = new List<Item>();
        BagItemList.AddRange(_base.BagConsumableList);
        BagItemList.AddRange(_base.BagEquipmentList);
        BagItemList.AddRange(_base.BagTreasureList);
        
        RunTable = new List<Command>(_base.RunTable ?? new List<Command>());
        DeckList = new List<Command>(_base.DeckList ?? new List<Command>());
        StorageList = new List<Command>(_base.StorageList ?? new List<Command>());
        Enchants = new List<Enchant>();

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
                if (damage.EnegyType == EnegyType.Life)
                {
                    Life = Life - damage.Val;
                }
                if (damage.EnegyType == EnegyType.Battery)
                {
                    Battery = Battery - damage.Val;
                }
                if (damage.EnegyType == EnegyType.Soul)
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

    public void DecreaseEnchant()
    {
        for (int i = Enchants.Count - 1; i >= 0; i--)
        {
            Enchants[i].Val -= 1;
            if (Enchants[i].Val <= 0)
            {
                Enchants.RemoveAt(i);
            }
        }
    }

    //　同じタイプの装備がある場合は、それを外してから追加して元の装備を返す
    // もともと装備をしていない場合はnullを返す
    public Equipment AddEquipment(Equipment equipment)
    {
        Equipment existingEquipment = EquipmentList.Find(e => e.EquipmentBase.EquipmentType == equipment.EquipmentBase.EquipmentType);
        if (existingEquipment != null)
        {
            EquipmentList.Remove(existingEquipment);
            EquipmentList.Add(equipment);
            return existingEquipment;
        }
        else
        {
            EquipmentList.Add(equipment);
            return null;
        }
    }

    public bool AddItem(Item item)
    {

        if (item is Consumable consumable)
        {
            if (PouchList.Count < Pouch.val)
            {
                PouchList.Add(consumable);
            }
            else if (BagItemList.Count < Bag.val)
            {
                BagItemList.Add(consumable);
            }
            else
            {
                Debug.Log("Pouch & Bag is full.");
                return false;
            }
        }
        else if (item is Equipment equipment)
        {
            if (BagItemList.Count < Bag.val)
            {
                BagItemList.Add(equipment);
            }
            else
            {
                Debug.Log("Bag is full.");
                return false;
            }
        }
        else if (item is Treasure treasure)
        {
            if (BagItemList.Count < Bag.val)
            {
                BagItemList.Add(treasure);
            }
            else
            {
                Debug.Log("Bag is full.");
                return false;
            }
        }
        else
        {
            Debug.Log("Bag is full.");
            return false;
        }
        return true;
    }

    public void AddEquipmentToBag(Equipment equipment)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        EquipmentList.Add(equipment); // Bag はプレイヤーのインベントリリスト
        BagItemList.Add(equipment); // Bag はプレイヤーのインベントリリスト
    }

    public void AddCommandToDeck(Command command)
    {
        // プレイヤーのインベントリにアイテムを追加する処理
        DeckList.Add(command); // Bag はプレイヤーのインベントリリスト
    }
}
