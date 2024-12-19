using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

public class WorldMapSystem : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;   // ベースレイヤーのTilemap
    [SerializeField] private Tilemap groundTilemap; // グラウンド描画用Tilemap
    [SerializeField] private Tilemap floorTilemap; // フロア描画用Tilemap
    [SerializeField] private Tilemap fieldTilemap; // フィールド描画用Tilemap
    [SerializeField] private Tilemap roadTilemap; // ロード描画用Tilemap
    [SerializeField] private Tilemap spotTilemap; // スポット描画用Tilemap
    [SerializeField] private GroundTileManager groundTileManager;
    [SerializeField] private FieldTileManager fieldTileManager;
    [SerializeField] private RoadTileManager roadTileManager;
    [SerializeField] private SpotTileManager spotTileManager;
    private string groundJsonData = "GroundTileMapData";
    private string floorJsonData = "FloorTileMapData";
    private string fieldJsonData = "FieldTileMapData";
    private string roadJsonData = "RoadTileMapData";
    private string spotJsonData = "SpotTileMapData";

    private void Start()
    {
        // マップデータを読み込んで描画
        // TODO：ひとまず描画の必要はないのでコメントアウト
        // RenderGroundMap();
        // RenderFloorMap();
        // RenderFieldMap();
        // RenderRoadMap();
        // RenderSpotMap();
    }

    // 現在のフィールドデータを取得する
    public FieldData getFieldData()
    {
        // 仮のFieldTypeを設定
        FieldType fieldType = (FieldType)1;

        // FieldDataを生成して値を設定
        FieldData fieldData = new FieldData
        {
            type = fieldType,
            row = 20,
            col = 20
        };

        return fieldData;
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
        TileMapData mapData = LoadJsonMapData(groundJsonData);

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
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    Debug.Log($"Tile at ({x}, {y}): {mapData.data[y][x]}");
                }
            }
        }
    }

    public void RenderFloorMap()
    {
        TileMapData floorData = LoadJsonMapData(floorJsonData);

        for (int y = 0; y < floorData.rows; y++)
        {
            for (int x = 0; x < floorData.cols; x++)
            {
                int floorID = floorData.data[y][x];
                // デフォルトフィールドタイプのフロアは無視する
                if (floorID != 0)
                {
                    // FieldTileManager または GetTile が null の場合に備えたチェック
                    Sprite floorSprite = fieldTileManager != null ? fieldTileManager.GetFloorTile((FieldType)floorID) : null;

                    if (floorSprite != null)
                    {
                        Tile floorTile = ScriptableObject.CreateInstance<Tile>();
                        floorTile.sprite = floorSprite;
                        floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                    }
                }
            }
        }
    }

    public void RenderFieldMap()
    {
        TileMapData fieldData = LoadJsonMapData(fieldJsonData);
        TileMapData floorData = LoadJsonMapData(floorJsonData);

        for (int y = 0; y < fieldData.rows; y++)
        {
            for (int x = 0; x < fieldData.cols; x++)
            {
                int fieldID = floorData.data[y][x];
                int tileID = fieldData.data[y][x];
                // デフォルトフィールドタイプのフロアは無視する
                if (tileID != 0)
                {
                    // FieldTileManager または GetTile が null の場合に備えたチェック
                    Sprite fieldSprite = fieldTileManager != null ? fieldTileManager.GetTile((FieldType)fieldID, tileID) : null;

                    if (fieldSprite != null)
                    {
                        Tile fieldTile = ScriptableObject.CreateInstance<Tile>();
                        fieldTile.sprite = fieldSprite;
                        fieldTilemap.SetTile(new Vector3Int(x, y, 0), fieldTile);
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
                        roadTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
    }

    public void RenderSpotMap()
    {
        TileMapData spotData = LoadJsonMapData(spotJsonData);

        for (int y = 0; y < spotData.rows; y++)
        {
            for (int x = 0; x < spotData.cols; x++)
            {
                int spotID = spotData.data[y][x];
                // スポットが敷かれていない場所は無視する
                if (spotID != 0)
                {
                    // FieldTileManager または GetTile が null の場合に備えたチェック
                    Sprite spotTile = spotTileManager != null ? spotTileManager.GetDefaultTile() : null;

                    if (spotTile != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = spotTile;
                        spotTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
    }
}
