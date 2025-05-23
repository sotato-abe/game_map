using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTarget", menuName = "Type/Target")]
public class TargetData : ScriptableObject
{
    public TargetType targetType;
    [SerializeField] public string enchantName;
    public Sprite icon;
    [TextArea] public string description;
}
