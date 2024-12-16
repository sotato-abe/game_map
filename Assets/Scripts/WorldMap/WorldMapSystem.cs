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
    [SerializeField] private RoadTileManager roadTileManager;
    private string groundJsonData = "GroundTileMapData";
    private string floorJsonData = "FloorTileMapData";
    private string fieldJsonData = "FieldTileMapData";
    private string roadJsonData = "RoadTileMapData";
    private string spotJsonData = "SpotTileMapData";

    private void Start()
    {
        // マップデータを読み込んで描画
        // RenderGroundMap();
        // RenderFieldMap();
        RenderRoadMap();
    }

    public TileMapData LoadJsonMapData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
        Debug.Log(filePath);

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

    public void RenderGroundMap()
    {
        TileMapData mapData = LoadJsonMapData(fieldJsonData);

        Debug.Log($"RenderMap:{mapData.rows}/{mapData.cols}");
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

    public void RenderFieldMap()
    {
        TileMapData floorData = LoadJsonMapData(floorJsonData);
        TileMapData fieldData = LoadJsonMapData(fieldJsonData);

        for (int y = 0; y < fieldData.rows; y++)
        {
            for (int x = 0; x < fieldData.cols; x++)
            {
                int fieldID = floorData.data[y][x];
                int tileID = fieldData.data[y][x];
                // デフォルトフィールドタイプのフロアは無視する
                if (fieldID != 0 && tileID != 0)
                {
                    // FieldTileManager または GetTile が null の場合に備えたチェック
                    Sprite fieldTile = fieldTileManager != null ? fieldTileManager.GetTile((FieldType)fieldID, tileID) : null;

                    if (fieldTile != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = fieldTile;
                        renderTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
    }

        public void RenderRoadMap()
    {
        TileMapData roadData = LoadJsonMapData(roadJsonData);

        for (int y = 0; y < roadData.rows; y++)
        {
            for (int x = 0; x < roadData.cols; x++)
            {
                int roadID = roadData.data[y][x];
                // 道が敷かれていない場所は無視する
                if (roadID != 0)
                {
                    // FieldTileManager または GetTile が null の場合に備えたチェック
                    Sprite roadTile = roadTileManager != null ? roadTileManager.GetTile((RoadType)roadID) : null;

                    if (roadTile != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = roadTile;
                        renderTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
    }
}
