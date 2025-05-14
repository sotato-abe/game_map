using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattlerData", menuName = "Field/FieldBattlerData")]
public class FieldBattlerData : ScriptableObject
{
    public FieldType fieldType;
    public List<Battler> battlerList;
}
