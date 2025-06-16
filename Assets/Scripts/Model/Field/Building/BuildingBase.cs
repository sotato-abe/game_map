using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building")]
public class BuildingBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite icon;
    [SerializeField] Sprite image;
    [SerializeField] Battler owner;
    [SerializeField, TextArea] string description;
    [SerializeField] List<Equipment> equipmentList;
    [SerializeField] List<Consumable> consumableList;
    [SerializeField] List<Command> commandList;
    [SerializeField] List<Treasure> treasureList;

    public virtual BuildingType type => BuildingType.Building;
    public string Name { get => name; }
    public Battler Owner { get => owner; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public Sprite Image { get => image; }

    public List<Consumable> ConsumableItems => consumableList;
    public List<Equipment> EquipmentItems => equipmentList;
    public List<Command> CommandItems => commandList;
    public List<Treasure> TreasureItems => treasureList;
}
