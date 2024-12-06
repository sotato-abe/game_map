using System.Collections.Generic;
using UnityEngine;

public class GroundTileManager : MonoBehaviour
{
    [SerializeField] private List<GroundTileBase> tiles; // 各タイルのリスト

    private Dictionary<GroundType, GroundTileBase> tileDict;

    private void Awake()
    {
        // Dictionary に変換して効率的に検索できるように
        tileDict = new Dictionary<GroundType, GroundTileBase>();
        foreach (var tile in tiles)
        {
            if (!tileDict.ContainsKey(tile.ID))
            {
                tileDict.Add(tile.ID, tile);
            }
        }
    }

    public GroundTileBase GetTile(GroundType tileType)
    {
        return tileDict != null && tileDict.ContainsKey(tileType)
            ? tileDict[tileType]
            : null;
    }
}
