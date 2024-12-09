using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

public class WorldMapSystem : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;   // ベースレイヤーのTilemap
    [SerializeField] private Tilemap renderTilemap; // 描画用Tilemap
    [SerializeField] private GroundTileManager groundTileManager;
    [SerializeField] private FieldTileManager fieldTileManager;
    // [SerializeField] private TileBase defaultTile;
    [SerializeField] private string fileName = "TileMapData.json";
    [SerializeField] private bool loadSwitch = true;

    private void Start()
    {
        if (loadSwitch)
        {
            // マップデータ読込
            OutputTileMapData();
        }
        else
        {
            // マップデータを読み込んで描画
            TileMapData mapData = LoadMapData();
            if (mapData != null)
            {
                Debug.Log(mapData);
                RenderMap(mapData);
            }
        }
    }

    /// <summary>
    /// TilemapのデータをJSON形式で出力
    /// </summary>
    public void OutputTileMapData()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("Tilemapが設定されていません。");
            return;
        }

        // Tilemapの範囲とデータ取得
        BoundsInt bounds = GetTilemapBounds(targetTilemap);
        List<int[]> tileData = GetTileData(bounds);

        // JSONデータ作成
        TileMapData mapData = new TileMapData(tileData);
        string jsonData = JsonConvert.SerializeObject(mapData, Formatting.Indented);

        // JSONを保存
        string filePath = Path.Combine(Application.persistentDataPath, "TileMapData.json");
        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"TileMapデータをJSON形式で保存しました: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSONファイルの保存に失敗しました: {e.Message}");
        }
    }

    /// <summary>
    /// マップデータを読込
    /// </summary>
    /// <returns></returns>
    public TileMapData LoadMapData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSONファイルが見つかりません: {filePath}");
            return null;
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);
            TileMapData mapData = JsonConvert.DeserializeObject<TileMapData>(jsonData);
            return mapData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"マップデータ読込エラー: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// マップを描画する
    /// </summary>
    public void RenderMap(TileMapData mapData)
    {
        for (int y = 0; y < mapData.rows; y++)
        {
            for (int x = 0; x < mapData.cols; x++)
            {
                int tileID = mapData.data[y][x];

                // GroundTileManager または GetTile が null の場合に備えたチェック
                GroundTileBase groundTile = groundTileManager != null ? groundTileManager.GetTile((GroundType)tileID) : null;

                if (groundTile != null)
                {
                    Tile tile = ScriptableObject.CreateInstance<Tile>();
                    tile.sprite = groundTile.Sprite;
                    renderTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    Debug.Log($"Tile at ({x}, {y}): {mapData.data[y][x]}");
                }
            }
        }
    }

    /// <summary>
    /// Tilemapの境界範囲を取得
    /// </summary>
    private BoundsInt GetTilemapBounds(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        return tilemap.cellBounds;
    }

    /// <summary>
    /// Tilemapのデータを取得
    /// </summary>
    private List<int[]> GetTileData(BoundsInt bounds)
    {
        List<int[]> tileData = new List<int[]>();
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            int[] row = new int[bounds.size.x];
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                row[x - bounds.xMin] = DetermineTileType(position);
            }
            tileData.Add(row);
        }
        return tileData;
    }

    /// <summary>
    /// タイルタイプを判定
    /// </summary>
    private int DetermineTileType(Vector3Int position)
    {
        if (targetTilemap.HasTile(position))
        {
            TileBase tile = targetTilemap.GetTile(position);
            return tile?.name switch
            {
                "The_Roguelike_1-13-10_Alpha_234" => 1,
                "The_Roguelike_1-13-10_Alpha_236" => 1,
                "The_Roguelike_1-13-10_Alpha_230" => 1,
                "The_Roguelike_1-13-10_Alpha_237" => 1,
                "The_Roguelike_1-13-10_Alpha_255" => 1,
                "The_Roguelike_1-13-10_Alpha_235" => 1,
                "The_Roguelike_1-13-10_Alpha_267" => 1,
                "The_Roguelike_1-13-10_Alpha_247" => 1,
                "The_Roguelike_1-13-10_Alpha_288" => 2,
                "The_Roguelike_1-13-10_Alpha_342" => 2,
                "The_Roguelike_1-13-10_Alpha_272" => 2,
                "The_Roguelike_1-13-10_Alpha_297" => 2,
                "The_Roguelike_1-13-10_Alpha_277" => 2,
                "The_Roguelike_1-13-10_Alpha_354" => 2,
                "The_Roguelike_1-13-10_Alpha_273" => 2,
                "The_Roguelike_1-13-10_Alpha_358" => 2,
                "The_Roguelike_1-13-10_Alpha_406" => 3,
                "The_Roguelike_1-13-10_Alpha_409" => 3,
                "The_Roguelike_1-13-10_Alpha_397" => 3,
                "The_Roguelike_1-13-10_Alpha_404" => 3,
                "The_Roguelike_1-13-10_Alpha_400" => 3,
                "The_Roguelike_1-13-10_Alpha_388" => 3,
                "The_Roguelike_1-13-10_Alpha_398" => 3,
                "The_Roguelike_1-13-10_Alpha_396" => 3,
                _ => 0
            };
        }

        return 0;
    }
}
