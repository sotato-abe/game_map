using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoadTileBase : ScriptableObject
{
    [SerializeField] RoadType id;
    [SerializeField] Sprite sprite;

    public RoadType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
