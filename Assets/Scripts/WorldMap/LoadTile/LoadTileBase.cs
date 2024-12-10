using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LoadTileBase : ScriptableObject
{
    [SerializeField] LoadTileType id;
    [SerializeField] Sprite sprite;

    public LoadTileType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
