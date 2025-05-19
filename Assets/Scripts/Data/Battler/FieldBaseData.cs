using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattlerData", menuName = "Field/FieldBaseData")]
public class FieldBaseData : ScriptableObject
{
    public FieldType fieldType;
    public List<Battler> battlerList;
    public List<Item> itemList;
    public FieldTileListBase tileList;    
}
