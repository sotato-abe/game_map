using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonをインポート

public class WorldMapSystem : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap; // 対象のTilemap
    [SerializeField] private Tilemap baseTilemap; // 対象のTilemap

    private void Start()
    {
        OutputTileMapData();
    }

    /// <summary>
    /// Tilemapを読み込んでJSON形式で出力
    /// </summary>
    public void OutputTileMapData()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("Tilemapが設定されていません。");
            return;
        }

        // Tilemapの範囲を取得
        groundTilemap.CompressBounds();
        BoundsInt bounds = groundTilemap.cellBounds;

        // 1次元配列のリストを用意（フラットな形式）
        List<int[]> tileData = new List<int[]>();
        int rows = bounds.size.y;
        int cols = bounds.size.x;

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            int[] row = new int[cols];  // 行データを保持する配列
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                bool hasGroundTile = groundTilemap.HasTile(position); // 地面タイルの有無をチェック
                bool hasBaseTile = baseTilemap.HasTile(position); // ベースタイルの有無をチェック
                int tileType = 1;
                if (hasGroundTile)
                {
                    TileBase tile = groundTilemap.GetTile(position); //
                    if (tile.name == "The_Roguelike_1-13-10_Alpha_488")
                    {
                        tileType = 3;
                    }
                }
                else if (hasBaseTile)
                {
                    TileBase tile = baseTilemap.GetTile(position); //
                    if (tile.name == "The_Roguelike_1-13-10_Alpha_621")
                    {
                        tileType = 1;
                    }
                    else if (tile.name == "The_Roguelike_1-13-10_Alpha_622")
                    {
                        tileType = 2;
                    }
                }
                row[x - bounds.xMin] = tileType;  // タイルがあれば1、なければ0を追加
            }
            tileData.Add(row);  // 1行分を追加
        }

        // TileMapDataを作成
        TileMapData tileMapData = new TileMapData(tileData); // フラットな配列リストを渡す

        // JSONに変換して出力（Newtonsoft.Jsonを使用）
        string jsonData = JsonConvert.SerializeObject(tileMapData, Formatting.Indented); // Newtonsoft.Jsonを使用
        Debug.Log($"tileMapData: {jsonData}");

        // JSONをファイルに保存
        string filePath = Path.Combine(Application.persistentDataPath, "TileMapData.json");
        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"ファイルに書き込みました: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ファイル書き込みに失敗しました: {e.Message}");
        }

        Debug.Log($"タイルマップデータをJSONとして出力しました: {filePath}");
    }
}
