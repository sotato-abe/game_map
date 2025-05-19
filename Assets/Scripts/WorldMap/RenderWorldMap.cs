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
    private Vector2Int playerCoordinate;
    private Tile PlayerTile;
    private List<Vector2Int> cityCoordinateData; // 街の座標リスト

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
                if (cityCoordinateData.Contains(new Vector2Int(x, y)))
                {
                    Tile cityTile = ScriptableObject.CreateInstance<Tile>();
                    cityTile.sprite = citySprite;
                    worldmap.SetTile(new Vector3Int(x, y, 0), cityTile);
                }
            }
        }
    }

    public void ChangePlayerCoordinate(Vector2Int newCoordinate)
    {
        if (newCoordinate == null)
        {
            return;
        }
        else if (playerCoordinate != null)
        {
            var oldPos = new Vector3Int(playerCoordinate.x, playerCoordinate.y, 1);
            var currentTile = worldmap.GetTile(oldPos);
            if (currentTile == PlayerTile)
            {
                worldmap.SetTile(oldPos, null);
            }
        }
        // 新しい位置にPlayerTileを置く
        var newPos = new Vector3Int(newCoordinate.x, newCoordinate.y, 1);
        worldmap.SetTile(newPos, PlayerTile);
        playerCoordinate = newCoordinate;

        // カメラを新しい位置に移動
        worldMapCameraManager.TargetPlayer(newPos);
    }
}
