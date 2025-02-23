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
    [SerializeField] int lifeCost;
    [SerializeField] int batteryCost;
    [SerializeField] int soulCost;
    [SerializeField] Probability probability; // TODO:１～１００のクラスにする
    [SerializeField] Sprite sprite;
    [SerializeField] private List<Skill> skillList = new List<Skill>();

    public string Name { get => name; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public int Attack { get => attack; }
    public int Technique { get => technique; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
    public Cost LifeCost => new Cost(CostType.Life, lifeCost);
    public Cost BatteryCost => new Cost(CostType.Battery, batteryCost);
    public Cost SoulCost => new Cost(CostType.Soul, soulCost);
    public Probability Probability { get => probability; }
    public Sprite Sprite { get => sprite; }
    public List<Skill> SkillList { get => skillList; }
    public List<Cost> CostList => new List<Cost>
    {
        LifeCost,
        BatteryCost,
        SoulCost,
    };
}
