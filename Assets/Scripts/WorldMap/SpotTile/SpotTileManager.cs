using System.Collections.Generic;
using UnityEngine;

public class SpotTileManager : MonoBehaviour
{
    [SerializeField] private List<SpotTileBase> tiles; // 各タイルのリスト

    private Dictionary<SpotType, SpotTileBase> tileDict;

    private void Awake()
    {
        // Dictionary に変換して効率的に検索できるように
        tileDict = new Dictionary<SpotType, SpotTileBase>();
        foreach (var tile in tiles)
        {
            if (!tileDict.ContainsKey(tile.ID))
            {
                tileDict.Add(tile.ID, tile);
            }
        }
    }

    public Sprite GetTile(SpotType tileType)
    {
        return tileDict != null && tileDict.ContainsKey(tileType)
            ? tileDict[tileType].Sprite
            : null;
    }

    public Sprite GetDefaultTile()
    {
        return tileDict[0]
            ? tileDict[0].Sprite
            : null;
    }
}
