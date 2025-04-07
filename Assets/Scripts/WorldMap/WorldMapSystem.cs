using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

// JSONのマップデータを読み込みフィールドデータを返す
// TODO：RenderWorldMapにワールドマップの表示をリクエストする。
public class WorldMapSystem : MonoBehaviour
{
    // GroundTileMapData:GroundTypeを使用する 遠洋：０、海：１、大地：２
    TileMapData groundData;
    TileMapData floorData;
    TileMapData roadData;
    TileMapData spotData;

    [SerializeField] List<MapBase> mapBaseList; // 地面と壁のプレファブ

    public FieldData fieldData; // フィールドデータ
    private Coordinate coordinate; // 座標

    void Start()
    {
        // 初期化処理
        groundData = LoadJsonMapData("GroundTileMapData");
        floorData = LoadJsonMapData("FloorTileMapData");
        roadData = LoadJsonMapData("RoadTileMapData");
        spotData = LoadJsonMapData("SpotTileMapData");
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

    // 指定座標からのフィールドデータを取得する
    // TODO：指定座標が海のとき、周りグランドがあるかを確認する。周りにグラウンドがあるときはその方向を返す。
    public FieldData getFieldDataByCoordinate(Coordinate targetCoordinate)
    {
        coordinate = targetCoordinate; // 座標を取得

        fieldData = new FieldData(); // フィールドデータを初期化
        fieldData.coordinate = targetCoordinate; // フィールドデータに座標を設定

        FindSpot(); // フィールドデータにスポットを設定
        if (fieldData.spot == null)
        {
            SetMapTileSet(); // フィールドデータにタイルセットを設定
            SetRoadEntry(); // フィールドデータに出入り口を設定
        }

        return fieldData;
    }

    private void FindSpot()
    {
        MapBase mapBase = mapBaseList.Find(m => m.Coordinate.col == coordinate.col && m.Coordinate.row == coordinate.row);

        if (mapBase != null)
        {
            fieldData.spot = mapBase;
        }
        else
        {
            Debug.LogWarning($"該当する MapBase が見つかりませんでした");
            fieldData.spot = null;
        }
    }

    private void SetMapTileSet()
    {
        FloorType floorType = (FloorType)floorData.data[coordinate.row][coordinate.col];

        fieldData.floorType = (int)floorType; // マップのタイルセットを取得
    }

    private void SetRoadEntry()
    {
        DirectionType roadType = (DirectionType)roadData.data[coordinate.row][coordinate.col];
        if (roadType == DirectionType.Top || roadType == DirectionType.TopLeft || roadType == DirectionType.TopRight || roadType == DirectionType.TopBottom || roadType == DirectionType.TopRightBottom || roadType == DirectionType.TopRightLeft || roadType == DirectionType.Cross)
        {
            fieldData.openTop = true; // 上の出入り口
        }
        else if (roadType == DirectionType.Left || roadType == DirectionType.TopLeft || roadType == DirectionType.RightLeft || roadType == DirectionType.BottomLeft || roadType == DirectionType.TopBottomLeft || roadType == DirectionType.Cross)
        {
            fieldData.openLeft = true; // 左の出入り口
        }
        else if (roadType == DirectionType.Right || roadType == DirectionType.RightBottom || roadType == DirectionType.RightLeft || roadType == DirectionType.TopRight || roadType == DirectionType.TopRightLeft || roadType == DirectionType.Cross)
        {
            fieldData.openRight = true; // 右の出入り口
        }
        else if (roadType == DirectionType.Bottom || roadType == DirectionType.BottomLeft || roadType == DirectionType.RightBottom || roadType == DirectionType.TopBottom || roadType == DirectionType.TopRightBottom || roadType == DirectionType.Cross)
        {
            fieldData.openBottom = true; // 下の出入り口
        }
    }
}
