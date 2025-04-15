using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/FloorTileList")]
public class FloorTileList : ScriptableObject
{
    [SerializeField] Sprite Default;
    [SerializeField] Sprite Desert;
    [SerializeField] Sprite Wilderness;
    [SerializeField] Sprite Grasslands;
    [SerializeField] Sprite Wetlands;
    [SerializeField] Sprite Snow;
    [SerializeField] Sprite Ice;
    [SerializeField] Sprite Magma;
    [SerializeField] Sprite Pollution;
    [SerializeField] Sprite Sea;

    public Sprite GetFloorTypeTile(FieldType type)
    {
        switch (type)
        {
            case FieldType.Default:
                return Default;
            case FieldType.Desert:
                return Desert;
            case FieldType.Wilderness:
                return Wilderness;
            case FieldType.Grasslands:
                return Grasslands;
            case FieldType.Wetlands:
                return Wetlands;
            case FieldType.Snow:
                return Snow;
            case FieldType.Ice:
                return Ice;
            case FieldType.Magma:
                return Magma;
            case FieldType.Pollution:
                return Pollution;
            default:
                return Sea; // デフォルトのタイルを返す
        }
    }
}
