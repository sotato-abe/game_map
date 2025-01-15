using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] int maxLife;
    [SerializeField] int maxBattery;
    [SerializeField] int attack;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] Sprite sprite;
    [SerializeField] int money;
    [SerializeField] int disk;
    [SerializeField] int key;
    [SerializeField] MapBase birthplace;

    [SerializeField] List<Equipment> equipments;
    [SerializeField] List<Item> items;
    [SerializeField] List<Command> comands;

    public string Name { get => name; }
    public int MaxLife { get => maxLife; }
    public int MaxBattery { get => maxBattery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public int Money { get => money; }
    public int Disk { get => disk; }
    public int Key { get => key; }
    public Sprite Sprite { get => sprite; }
    public List<Equipment> Equipments { get => equipments; }
    public List<Item> Items { get => items; }
    public List<Command> Commands { get => comands; }
}
