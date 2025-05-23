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

    public Status Power { get; set; }
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
    public List<Enchant> Enchants = new List<Enchant>();
    public Vector2Int coordinate;

    public List<Status> StatusList => new List<Status>
    {
        Power,
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
        Power = _base.Power;
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
    public void TakeAttack(List<Attack> attacks)
    {
        // attacksをDamageList、RecoveryList、EnchantListでまとめる
        List<Enegy> damageList = new List<Enegy>();
        List<Enegy> recoveryList = new List<Enegy>();
        List<Enchant> enchantList = new List<Enchant>();
        foreach (Attack attack in attacks)
        {
            damageList.AddRange(attack.DamageList);
            recoveryList.AddRange(attack.RecoveryList);
            enchantList.AddRange(attack.EnchantList);

            Debug.Log($"attack: {attack.Target}");
            Debug.Log($"damageList: {attack.DamageList.Count}");
            Debug.Log($"recoveryList: {attack.RecoveryList.Count}");
            Debug.Log($"enchantList: {attack.EnchantList.Count}");
        }
        TakeEnegy(damageList, true);
        TakeRecovery(recoveryList);
        TakeEnchant(enchantList);
    }

    private void TakeEnegy(List<Enegy> enegryList, bool isDown)
    {
        int operatorVal = isDown ? -1 : 1;
        foreach (Enegy enegry in enegryList)
        {
            if (enegry.type == EnegyType.Life)
            {
                Life += operatorVal * enegry.val;
                Life = Mathf.Min(Life, MaxLife);
            }
            if (enegry.type == EnegyType.Battery)
            {
                Battery += operatorVal * enegry.val;
                Battery = Mathf.Min(Battery, MaxBattery);

            }
            if (enegry.type == EnegyType.Soul)
            {
                Soul += operatorVal * enegry.val;
                Soul = Mathf.Min(Soul, 100);
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
}
