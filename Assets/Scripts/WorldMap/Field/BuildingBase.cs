using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/BuildingBase")]
public class BuildingBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] BuildingType type;
    [SerializeField] Battler owner;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] List<Item> items;

    public string Name { get => name; }
    public BuildingType Type { get => type; }
    public Battler Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public List<Item> Items { get => items; }
}
