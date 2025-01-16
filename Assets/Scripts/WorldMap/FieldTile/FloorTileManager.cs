using System.Collections.Generic;
using UnityEngine;

public class FloorTileManager : MonoBehaviour
{
    [SerializeField] private List<FloorTileListBase> tileLists;

    public FloorTileListBase GetTileListByID(int id)
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

    public Sprite GetTile(FloorType floorType, int tileType)
    {
        // floorType に一致する FloorTileListBase を取得
        var floorTileList = tileLists[(int)floorType];

        if (floorTileList == null)
        {
            Debug.LogError($"FieldTileList not found for FloorType: {floorType}");
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

    public Sprite GetFloorTile(FloorType floorType)
    {
        if (floorType == 0)
        {
            return null;
        }

        var fieldTileList = tileLists[(int)floorType];
        if (fieldTileList == null)
        {
            Debug.LogError($"FieldTileList not found for FloorType: {floorType}");
            return null;
        }
        return fieldTileList.Floor;
    }
}
