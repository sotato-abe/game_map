using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatus", menuName = "Status/Status Data")]
public class StatusData : ScriptableObject
{
    public StatusType statusType;
    [SerializeField] public string statusName;
    public Sprite icon;
    [TextArea] public string description;
}
