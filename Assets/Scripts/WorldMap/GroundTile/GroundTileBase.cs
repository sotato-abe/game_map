using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/GroundTileBase")]
public class GroundTileBase : ScriptableObject
{
    [SerializeField] GroundType id;
    [SerializeField] Sprite sprite;

    public GroundType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
