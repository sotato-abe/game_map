using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battler/BattlerGroup")]
public class BattlerGroup : ScriptableObject
{
    [SerializeField] List<Battler> BattlerList;
}
