using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentBase : ItemBase
{
    public override ItemType itemType => ItemType.Equipment;
    [SerializeField] EquipmentType type;
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int attack;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;

    public EquipmentType Type { get => type; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
}
