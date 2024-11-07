using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] int attack;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] Sprite sprite;


    public string Name { get => name; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public Sprite Sprite { get => sprite; }
}
