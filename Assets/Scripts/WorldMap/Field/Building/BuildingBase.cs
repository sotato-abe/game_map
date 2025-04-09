using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/BuildingBase")]
public class BuildingBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Battler owner;
    [SerializeField] string description;
    [SerializeField] Sprite icon;

    public string Name { get => name; }
    public Battler Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
}
