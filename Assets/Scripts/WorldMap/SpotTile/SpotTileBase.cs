using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpotTileBase : ScriptableObject
{
    [SerializeField] SpotTileType id;
    [SerializeField] Sprite sprite;

    public SpotTileType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
