using System.Collections.Generic;
using UnityEngine;

public class FloorTileManager : MonoBehaviour
{
    [SerializeField] private List<FieldTileListBase> tileLists;

    public FieldTileListBase GetTileListByID(int id)
    {
        foreach (var tileList in tileLists)
        {
            if ((int)tileList.Type == id) // IDプロパティを使用
            {
                return tileList;
            }
        }
        return null;
    }

    public Sprite GetTile(FieldType fieldType, int tileType)
    {
        // fieldType に一致する FieldTileListBase を取得
        var floorTileList = tileLists[(int)fieldType];

        if (floorTileList == null)
        {
            Debug.LogError($"FieldTileList not found for FieldType: {fieldType}");
            return null;
        }

        // index に対応するタイルを返す
        return tileType switch
        {
            0 => floorTileList.Floor,
            1 => floorTileList.Tree,
            2 => floorTileList.Grass,
            3 => floorTileList.Rock,
            _ => throw new System.ArgumentOutOfRangeException($"Invalid tileType: {tileType}. Must be between 0 and 3.")
        };
    }

    public Sprite GetFloorTile(FieldType fieldType)
    {
        if (fieldType == 0)
        {
            return null;
        }

        var fieldTileList = tileLists[(int)fieldType];
        if (fieldTileList == null)
        {
            Debug.LogError($"FieldTileList not found for FieldType: {fieldType}");
            return null;
        }
        return fieldTileList.Floor;
    }
}
