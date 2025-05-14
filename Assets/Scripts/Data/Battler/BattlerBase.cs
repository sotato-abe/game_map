using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battler/BattlerBase")]
public class BattlerBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] MapBase birthplace;
    [SerializeField] int maxLife;
    [SerializeField] int maxBattery;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int technique;
    [SerializeField] int speed;
    [SerializeField] int luck;
    [SerializeField] int memory = 5;
    [SerializeField] int storage = 10;
    [SerializeField] int pouch = 5;
    [SerializeField] int bag = 10;
    [SerializeField] int money = 10;
    [SerializeField] int disk = 5;
    [SerializeField] int key = 5;
    [SerializeField] int exp = 5;

    [SerializeField] List<Ability> abilityList;
    [SerializeField] List<Equipment> equipments;
    [SerializeField] List<Command> runTable;
    [SerializeField] List<Command> deckList;
    [SerializeField] List<Command> storageList;
    [SerializeField] List<Item> pouchList;
    [SerializeField] List<Item> bagItemList;
    [SerializeField] List<Equipment> bagEquipmentList;
    [SerializeField] List<TalkMessage> messages;

    public string Name { get => name; }
    public Sprite Sprite { get => sprite; }
    public int MaxLife { get => maxLife; }
    public int MaxBattery { get => maxBattery; }

    public Status Attack => new Status(StatusType.ATK, attack);
    public Status Technique => new Status(StatusType.TEC, technique);
    public Status Defense => new Status(StatusType.DEF, defense);
    public Status Speed => new Status(StatusType.SPD, speed);
    public Status Luck => new Status(StatusType.LUK, luck);
    public Status Memory => new Status(StatusType.MMR, memory);
    public Status Storage => new Status(StatusType.STG, storage);
    public Status Pouch => new Status(StatusType.POC, pouch);
    public Status Bag => new Status(StatusType.BAG, bag);

    public int Money { get => money; }
    public int Disk { get => disk; }
    public int Key { get => key; }
    public int Exp { get => exp; }

    public List<Ability> AbilityList { get => abilityList; }
    public List<Equipment> Equipments { get => equipments; }
    public List<Item> PouchList { get => pouchList; }
    public List<Item> BagItemList { get => bagItemList; }
    public List<Equipment> BagEquipmentList { get => bagEquipmentList; }
    public List<Command> RunTable { get => runTable; }
    public List<Command> DeckList { get => deckList; }
    public List<Command> StorageList { get => storageList; }
    public List<TalkMessage> Messages { get => messages; }
    public MapBase Birthplace { get => birthplace; }
}
