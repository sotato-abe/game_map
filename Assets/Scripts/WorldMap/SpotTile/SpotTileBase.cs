using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpotTileBase : ScriptableObject
{
    [SerializeField] SpotType id;
    [SerializeField] Sprite sprite;

    public SpotType ID { get => id; }
    public Sprite Sprite { get => sprite; }
}
