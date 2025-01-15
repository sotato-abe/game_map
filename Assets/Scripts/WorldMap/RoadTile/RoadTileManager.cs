using System.Collections.Generic;
using UnityEngine;

public class RoadTileManager : MonoBehaviour
{
    [SerializeField] private List<RoadTileBase> tiles; // 各タイルのリスト

    private Dictionary<DirectionType, RoadTileBase> tileDict;

    private void Awake()
    {
        // Dictionary に変換して効率的に検索できるように
        tileDict = new Dictionary<DirectionType, RoadTileBase>();
        foreach (var tile in tiles)
        {
            if (!tileDict.ContainsKey(tile.ID))
            {
                tileDict.Add(tile.ID, tile);
            }
        }
    }

    public Sprite GetTile(DirectionType tileType)
    {
        return tileDict != null && tileDict.ContainsKey(tileType)
            ? tileDict[tileType].Sprite
            : null;
    }
}
