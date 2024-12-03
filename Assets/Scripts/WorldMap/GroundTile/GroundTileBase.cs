using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GroundTileBase : ScriptableObject
{
    [SerializeField] GroundTileType id;
    [SerializeField] Sprite sprite;

    public GroundTileType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
