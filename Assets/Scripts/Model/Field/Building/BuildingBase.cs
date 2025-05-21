using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building")]
public class BuildingBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite icon;
    [SerializeField] Battler owner;
    [SerializeField, TextArea] string description;

    public virtual BuildingType type => BuildingType.Building;
    public string Name { get => name; }
    public Battler Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
}
