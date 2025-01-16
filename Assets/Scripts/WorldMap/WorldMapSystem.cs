using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

public class WorldMapSystem : MonoBehaviour
{
    // GroundTileMapData:GroundTypeを使用する 遠洋：０、海：１、大地：２
    private string groundJsonData = "GroundTileMapData";
    private string floorJsonData = "FloorTileMapData";
    private string roadJsonData = "RoadTileMapData";
    private string spotJsonData = "SpotTileMapData";

    // 指定座標からのフィールドデータを取得する
    // 指定座標が海のとき、周りグランドがあるかを確認する。周りにグラウンドがあるときはその方向を返す。
    public FieldData getFieldDataByCoordinate(Coordinate targetCoordinate) //targetCoordinate : {row = 20,col = 20};
    {
        TileMapData groundData = LoadJsonMapData(groundJsonData);
        TileMapData floorData = LoadJsonMapData(floorJsonData);
        TileMapData roadData = LoadJsonMapData(roadJsonData);
        TileMapData spotData = LoadJsonMapData(spotJsonData);
        GroundType groundType = (GroundType)groundData.data[targetCoordinate.row][targetCoordinate.col];
        FloorType floorType = (FloorType)floorData.data[targetCoordinate.row][targetCoordinate.col];
        DirectionType roadType = (DirectionType)roadData.data[targetCoordinate.row][targetCoordinate.col];
        SpotType spotNum = (SpotType)spotData.data[targetCoordinate.row][targetCoordinate.col];

        // FieldDataを生成して値を設定
        FieldData fieldData = new FieldData
        {
            groundType = groundType,
            floorType = floorType,
            roadDirection = roadType,
            spot = spotNum,
            coordinate = targetCoordinate // 仮の座標を設定
        };

        return fieldData;
    }

    private TileMapData LoadJsonMapData(string fileName)
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
}
