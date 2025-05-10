using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    [SerializeField] AbilityBase _base;

    public AbilityBase Base { get => _base; }
    public string Name { get => Base.Name; }
    public RarityType Rarity { get => Base.Rarity; }
    public Sprite Sprite { get => Base.Sprite; }
    public string Description { get => Base.Description; }
}
