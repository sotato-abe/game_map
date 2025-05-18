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
    [SerializeField] Sprite seaTile; // プレイヤーの位置を示すスプライト
    [SerializeField] Sprite oceanTile; // プレイヤーの位置を示すスプライト
    [SerializeField] Sprite citySprite; // プレイヤーの位置を示すスプライト
    [SerializeField] WorldMapCameraManager worldMapCameraManager; // カメラ
    private string floorJsonData = "FloorTileMapData";
    private Coordinate playerCoordinate;
    private Tile PlayerTile;
    private List<Coordinate> cityCoordinateData; // 街の座標リスト

    private void Awake()
    {
        worldmap.ClearAllTiles();
        PlayerTile = ScriptableObject.CreateInstance<Tile>();
        PlayerTile.sprite = playerPositionSprite;
        cityCoordinateData = MapDatabase.Instance?.GetMapCoordinateList();
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
        TileMapData mapData = LoadJsonMapData(floorJsonData);

        for (int y = 0; y < mapData.rows; y++)
        {
            for (int x = 0; x < mapData.cols; x++)
            {
                int fieldTypeID = mapData.data[y][x];
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = floorTileList.GetFloorTypeTile((FieldType)fieldTypeID);
                if (tile.sprite)
                {
                    worldmap.SetTile(new Vector3Int(x, y, 0), tile);
                }
                // cityCoordinateDataの座標が一致する時にcityTileを描画
                if (x == 18 && y == 49)
                {
                    Debug.Log("City Coordinate: 18, 49 ----------------------");
                }
                if (cityCoordinateData.Contains(new Coordinate(y, x)))
                {
                    Debug.Log($"City Coordinate---------: {y}, {x}");
                    Tile cityTile = ScriptableObject.CreateInstance<Tile>();
                    cityTile.sprite = citySprite;
                    worldmap.SetTile(new Vector3Int(x, y, 0), cityTile);
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

        // カメラを新しい位置に移動
        worldMapCameraManager.TargetPlayer(newPos);
    }
}
