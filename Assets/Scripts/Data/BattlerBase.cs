using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] int maxHP;
    [SerializeField] int maxBattery;
    [SerializeField] int attack;
    [SerializeField] int technique;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] Sprite sprite;

    public string Name { get => name; }
    public int MaxHP { get => maxHP; }
    public int MaxBattery { get => maxBattery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }

    public Sprite Sprite { get => sprite; }
}
