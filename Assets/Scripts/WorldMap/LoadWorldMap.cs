using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

//TileMapを読み込んでJSONを出力するクラス
public class LoadWorldMap : MonoBehaviour
{
    [SerializeField] private Tilemap fieldTileMap; // 対象のTilemap

    [SerializeField] Sprite Default;        // デフォルト 488
    [SerializeField] Sprite Desert;         // 砂漠 447
    [SerializeField] Sprite Wilderness;     // 荒野 451
    [SerializeField] Sprite Grasslands;     // 草原 443
    [SerializeField] Sprite Wetlands;       // 湿地 441
    [SerializeField] Sprite Snow;           // 雪 631
    [SerializeField] Sprite Ice;            // 氷床 629
    [SerializeField] Sprite Magma;          // マグマ 603
    [SerializeField] Sprite Pollution;      // 汚染 437
    [SerializeField] Sprite Sea;        // 海 622
    [SerializeField] Sprite Ocean;      // 遠洋 621

    private void Start()
    {
        // OutputTileMapData();
    }

    public void OutputTileMapData()
    {
        if (fieldTileMap == null)
        {
            Debug.LogError("Tilemapが設定されていません。");
            return;
        }

        // Tilemapの範囲を取得
        fieldTileMap.CompressBounds();
        BoundsInt bounds = fieldTileMap.cellBounds;

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
                bool hasTile = fieldTileMap.HasTile(position); // タイルの有無をチェック
                int tileType = 10;
                if (hasTile)
                {
                    Tile tile = fieldTileMap.GetTile<Tile>(position);
                    string tileName = tile.sprite.name;
                    if (tileName == Default.name)
                        tileType = 0;
                    else if (tileName == Desert.name)
                        tileType = 1;
                    else if (tileName == Wilderness.name)
                        tileType = 2;
                    else if (tileName == Grasslands.name)
                        tileType = 3;
                    else if (tileName == Wetlands.name)
                        tileType = 4;
                    else if (tileName == Snow.name)
                        tileType = 5;
                    else if (tileName == Ice.name)
                        tileType = 6;
                    else if (tileName == Magma.name)
                        tileType = 7;
                    else if (tileName == Pollution.name)
                        tileType = 8;
                    else if (tileName == Sea.name)
                        tileType = 9;
                    else if (tileName == Ocean.name)
                        tileType = 10;
                    else
                        tileType = 10;
                }
                row[x - bounds.xMin] = tileType;  // タイルがあれば1、なければ0を追加
            }
            tileData.Add(row);  // 1行分を追加
        }

        // TileMapDataを作成
        TileMapData tileMapData = new TileMapData(tileData); // フラットな配列リストを渡す

        // JSONに変換して出力（Newtonsoft.Jsonを使用）
        string jsonData = JsonConvert.SerializeObject(tileMapData, Formatting.Indented); // Newtonsoft.Jsonを使用

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
