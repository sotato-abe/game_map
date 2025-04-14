using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

public class RenderWorldMap : MonoBehaviour
{
    [SerializeField] private Tilemap worldmap; // グラウンド描画用Tilemap
    [SerializeField] FloorTileList floorTileList;
    [SerializeField] Sprite playerPositionSprite; // プレイヤーの位置を示すスプライト
    private string GroundJsonData = "GroundTileMapData";
    private string floorJsonData = "FloorTileMapData";
    private Coordinate playerCoordinate;
    private Tile PlayerTile;

    private void Awake()
    {
        worldmap.ClearAllTiles();
        PlayerTile = ScriptableObject.CreateInstance<Tile>();
        PlayerTile.sprite = playerPositionSprite;
        RenderMap();
    }

    private TileMapData LoadJsonMapData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

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

    public void RenderMap()
    {
        // マップデータを読み込んで描画
        TileMapData groundmapData = LoadJsonMapData(GroundJsonData);
        TileMapData mapData = LoadJsonMapData(floorJsonData);

        if (groundmapData == null || mapData == null)
        {
            Debug.LogError("マップデータ読み込み失敗");
            return;
        }

        for (int y = 0; y < mapData.rows; y++)
        {
            for (int x = 0; x < mapData.cols; x++)
            {
                int fieldTypeID = mapData.data[y][x];
                int groundTypeID = groundmapData.data[y][x];

                if (fieldTypeID != null && groundTypeID == (int)GroundType.Ground)
                {
                    Tile tile = ScriptableObject.CreateInstance<Tile>();
                    tile.sprite = floorTileList.GetFloorTypeTile((FieldType)fieldTypeID);
                    if (tile.sprite)
                    {
                        worldmap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
    }

    public void ChangePlayerCoordinate(Coordinate newCoordinate)
    {
        if (newCoordinate == null)
        {
            return;
        }
        else if (playerCoordinate != null)
        {
            var oldPos = new Vector3Int(playerCoordinate.col, playerCoordinate.row, 1);
            var currentTile = worldmap.GetTile(oldPos);
            if (currentTile == PlayerTile)
            {
                worldmap.SetTile(oldPos, null);
            }
        }
        // 新しい位置にPlayerTileを置く
        playerCoordinate = new Coordinate(newCoordinate);
        var newPos = new Vector3Int(playerCoordinate.col, playerCoordinate.row, 1);
        worldmap.SetTile(newPos, PlayerTile);
    }
}
