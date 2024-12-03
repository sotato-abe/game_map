using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FieldTileListBase : ScriptableObject
{
    [SerializeField] FloorTileType type;
    [SerializeField] Sprite floor;
    [SerializeField] Sprite tree;
    [SerializeField] Sprite grass;
    [SerializeField] Sprite rock;

    public FloorTileType Type { get => type; }
    public Sprite Floor { get => floor; }
    public Sprite Tree { get => tree; }
    public Sprite Grass { get => grass; }
    public Sprite Rock { get => rock; }
}
