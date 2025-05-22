using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattlerData", menuName = "Field/FieldBaseData")]
public class FieldBaseData : ScriptableObject
{
    public FieldType fieldType;
    public List<Battler> battlerList;
    public List<Consumable> consumables;
    public List<Equipment> equipments;
    public List<Treasure> treasures;
    public FieldTileListBase tileList;
}
