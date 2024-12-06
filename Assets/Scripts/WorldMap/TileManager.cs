using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private List<GroundTileBase> tiles; // 各タイルのリスト

    private Dictionary<GroundTileType, GroundTileBase> tileDict;

    private void Awake()
    {
        // Dictionary に変換して効率的に検索できるように
        tileDict = new Dictionary<GroundTileType, GroundTileBase>();
        foreach (var tile in tiles)
        {
            if (!tileDict.ContainsKey(tile.ID))
            {
                tileDict.Add(tile.ID, tile);
            }
        }
    }

    public GroundTileBase GetTile(GroundTileType tileType)
    {
        return tileDict != null && tileDict.ContainsKey(tileType)
            ? tileDict[tileType]
            : null;
    }
}
