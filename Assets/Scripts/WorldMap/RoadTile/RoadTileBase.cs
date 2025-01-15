using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoadTileBase : ScriptableObject
{
    [SerializeField] DirectionType id;
    [SerializeField] Sprite sprite;

    public DirectionType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
