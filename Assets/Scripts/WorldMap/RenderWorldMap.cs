using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

public class RenderWorldMap : MonoBehaviour
{
    // [SerializeField] private Tilemap floorTilemap; // グラウンド描画用Tilemap
    [SerializeField] List<FieldTileListBase> FieldTileList; // 地面と壁のプレファブ 
    private string floorJsonData = "FloorTileMapData";

    public void RenderMap()
    {
        // マップデータを読み込んで描画
        RenderFloorMap();
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
        TileMapData mapData = LoadJsonMapData(floorJsonData);

        for (int y = 0; y < mapData.rows; y++)
        {
            for (int x = 0; x < mapData.cols; x++)
            {
                int fieldTypeID = mapData.data[y][x];

                if (fieldTypeID != null)
                {
                    // Tile tile = ScriptableObject.CreateInstance<Tile>();
                    // tile.sprite = floorTile.Sprite;
                    // floorTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }
}
