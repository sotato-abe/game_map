using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/FieldTypeTileList")]
public class FieldTypeTileList : ScriptableObject
{
    [SerializeField] FieldType type;
    [SerializeField] Sprite floor;
    [SerializeField] Sprite tree;
    [SerializeField] Sprite grass;
    [SerializeField] Sprite rock;

    public FieldType Type { get => type; }
    public Sprite Floor { get => floor; }
    public Sprite Tree { get => tree; }
    public Sprite Grass { get => grass; }
    public Sprite Rock { get => rock; }
}
