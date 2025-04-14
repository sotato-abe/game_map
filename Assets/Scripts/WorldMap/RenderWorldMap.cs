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

    private Coordinate playerCoordinate; // 座標

    public void RenderMap()
    {
        // マップデータを読み込んで描画
        RenderFloorMap();
    }

    public void SetPlayerCoordinate(Coordinate coordinate)
    {
        playerCoordinate = coordinate;
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

    public void RenderFloorMap()
    {
        TileMapData groundmapData = LoadJsonMapData(GroundJsonData);
        TileMapData mapData = LoadJsonMapData(floorJsonData);

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
                if (playerCoordinate != null)
                {
                    Debug.Log("mapdata:" + mapData.cols + ", " + mapData.rows);
                    if (x == playerCoordinate.col && y == playerCoordinate.row)
                    {
                        Debug.Log("PlayerCoordinate: " + playerCoordinate.row + ", " + playerCoordinate.col);
                        Tile tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = playerPositionSprite;
                        worldmap.SetTile(new Vector3Int(x, y, 1), tile);
                    }
                }
            }
        }
    }
}
