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

    // BattlerStatus
    public int Soul { get => soul; set => soul = value; }
    public int MaxLife { get; set; }
    public int ColLife { get; set; }
    public int Life { get; set; }
    public int MaxBattery { get; set; }
    public int ColBattery { get; set; }
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

    public Status ColPower = new Status(StatusType.POW, 0);
    public Status ColTechnique = new Status(StatusType.TEC, 0);
    public Status ColDefense = new Status(StatusType.DEF, 0);
    public Status ColSpeed = new Status(StatusType.SPD, 0);
    public Status ColLuck = new Status(StatusType.LUK, 0);
    public Status ColMemory = new Status(StatusType.MMR, 0);
    public Status ColStorage = new Status(StatusType.STG, 0);
    public Status ColPouch = new Status(StatusType.POC, 0);
    public Status ColBag = new Status(StatusType.BAG, 0);

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

    public List<EnegyCount> EnegyCountList => new List<EnegyCount>
    {
        new EnegyCount(EnegyType.Life, Life, MaxLife, ColLife),
        new EnegyCount(EnegyType.Battery, Battery, MaxBattery, ColBattery),
        // new EnegyCount(EnegyType.Soul, Soul, 100, 100),
    };

    public List<StatusCount> StatusCountList => new List<StatusCount>
    {
        new StatusCount(StatusType.POW, Power.val, ColPower.val),
        new StatusCount(StatusType.TEC, Technique.val, ColTechnique.val),
        new StatusCount(StatusType.DEF, Defense.val, ColDefense.val),
        new StatusCount(StatusType.SPD, Speed.val, ColSpeed.val),
        new StatusCount(StatusType.LUK, Luck.val, ColLuck.val),
        new StatusCount(StatusType.MMR, Memory.val, ColMemory.val),
        new StatusCount(StatusType.STG, Storage.val, ColStorage.val),
        new StatusCount(StatusType.POC, Pouch.val, ColPouch.val),
        new StatusCount(StatusType.BAG, Bag.val, ColBag.val),
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
        CoLStatus();
    }

    // TODO : Battlerのステータスを更新するメソッド
    public void CoLStatus()
    {
        int DiffLife = 0;
        int DiffBattery = 0;
        int DiffPower = 0;
        int DiffTechnique = 0;
        int DiffDefense = 0;
        int DiffSpeed = 0;
        int DiffLuck = 0;
        int DiffMemory = 0;
        int DiffStorage = 0;
        int DiffPouch = 0;
        int DiffBag = 0;

        foreach (Equipment equipment in EquipmentList)
        {
            DiffLife += equipment.EquipmentBase.Life;
            DiffBattery += equipment.EquipmentBase.Battery;
            DiffPower += equipment.EquipmentBase.Power;
            DiffTechnique += equipment.EquipmentBase.Technique;
            DiffDefense += equipment.EquipmentBase.Defense;
            DiffSpeed += equipment.EquipmentBase.Speed;
            DiffLuck += equipment.EquipmentBase.Luck;
            DiffMemory += equipment.EquipmentBase.Memory;
            DiffStorage += equipment.EquipmentBase.Storage;
            DiffPouch += equipment.EquipmentBase.Pouch;
            DiffBag += equipment.EquipmentBase.Bag;
        }

        ColLife = MaxLife + DiffLife;
        ColBattery = MaxBattery + DiffBattery;
        ColPower.val = Power.val + DiffPower;
        ColTechnique.val = Technique.val + DiffTechnique;
        ColDefense.val = Defense.val + DiffDefense;
        ColSpeed.val = Speed.val + DiffSpeed;
        ColLuck.val = Luck.val + DiffLuck;
        ColMemory.val = Memory.val + DiffMemory;
        ColStorage.val = Storage.val + DiffStorage;
        ColPouch.val = Pouch.val + DiffPouch;
        ColBag.val = Bag.val + DiffBag;
    }

    // Attak時のフィジカルダメージを取得
    public Attack GetPhysicalAttack()
    {
        Attack attack = new Attack(
            TargetType.EnemyFront,
            new List<Enegy>(),
            new List<Enegy>(),
            new List<Enchant>()
        );
        attack.DamageList.Add(new Enegy(EnegyType.Life, ColPower.val));
        return attack;
    }

    // ライフを割り切るときにfalseを返す（isAlive）
    public void TakeAttack(Attack attack)
    {
        TakeEnegy(attack.DamageList, true);
        TakeEnegy(attack.RecoveryList, false);
        TakeEnchant(attack.EnchantList);
    }

    public void TakeEnegy(List<Enegy> enegryList, bool isDown)
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
        switch (item)
        {
            case Consumable consumable:
                if (PouchList.Count < Pouch.val)
                {
                    PouchList.Add(consumable);
                    return true;
                }
                else
                {
                    return TryAddToBag(consumable);
                }

            case Equipment equipment:
                return TryAddToBag(equipment);

            case Treasure treasure:
                return TryAddToBag(treasure);

            default:
                Debug.Log("Unknown item type.");
                return false;
        }
    }

    private bool TryAddToBag(Item item)
    {
        if (BagItemList.Count < Bag.val)
        {
            BagItemList.Add(item);
            return true;
        }

        Debug.Log("バッグがいっぱいです。");
        return false;
    }

    public void UseConsumable(Consumable consumable)
    {
        if (PouchList.Contains(consumable))
        {
            PouchList.Remove(consumable);
            TakeAttack(consumable.Attack);
        }
        else
        {
            Debug.Log("そのアイテムはポーチにありません。");
        }
    }
}
