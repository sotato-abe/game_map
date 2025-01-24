using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GroundTileBase : ScriptableObject
{
    [SerializeField] GroundType id;
    [SerializeField] Sprite sprite;

    public GroundType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
