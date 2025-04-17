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

    [SerializeField] RenderWorldMap renderWorldMap;
    [SerializeField] List<MapBase> mapBaseList; // 地面と壁のプレファブ 
    [SerializeField] WorldMapPanel worldMapPanel;

    public FieldData fieldData; // フィールドデータ
    private Coordinate coordinate; // 座標
    public int worldWidth; // ワールドマップの幅
    public int worldHeight; // ワールドマップの高さ
    private bool isWorldMapShow = false; // ワールドマップの高さ

    void Awake()
    {
        // 初期化処理
        groundData = LoadJsonMapData("GroundTileMapData");
        floorData = LoadJsonMapData("FloorTileMapData");
        roadData = LoadJsonMapData("RoadTileMapData");
        spotData = LoadJsonMapData("SpotTileMapData");

        worldWidth = groundData.data[0].Length; // ワールドマップの幅を取得
        worldHeight = groundData.data.Count; // ワールドマップの高さを取得
        renderWorldMap.ChangePlayerCoordinate(coordinate); // ワールドマップを描画
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
        if (fieldData.mapBase == null)
        {
            SetMapTileSet(); // フィールドデータにタイルセットを設定
            SetRoadEntry(); // フィールドデータに出入り口を設定
        }
        SeachAroundGround();

        // TODO : 別処理を作ってそこでWorldMapの更新をする:
        renderWorldMap.ChangePlayerCoordinate(coordinate); // ワールドマップのPlayer位置を更新

        return fieldData;
    }

    private void FindSpot()
    {
        // Findを使用して一致するMapBaseを探す
        MapBase mapBase = mapBaseList.Find(m => m != null && m.Coordinate != null && m.Coordinate.col == coordinate.col && m.Coordinate.row == coordinate.row);

        // mapBaseが見つかった場合
        if (mapBase != null)
        {
            fieldData.mapBase = mapBase;
        }
        else
        {
            // Debug.LogWarning($"該当する MapBase が見つかりませんでした。座標: row={coordinate.row}, col={coordinate.col}");
            fieldData.mapBase = null; // フィールドデータにnullを設定
        }
    }

    private void SetMapTileSet()
    {
        FieldType fieldType = (FieldType)floorData.data[coordinate.row][coordinate.col];
        fieldData.fieldType = (int)fieldType; // マップのタイルセットを取得
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


    // 周囲のGroundを取得する
    private void SeachAroundGround()
    {
        bool isTopGround = groundData.data[coordinate.row + 1][coordinate.col] == (int)GroundType.Ground;
        bool isBottomGround = groundData.data[coordinate.row - 1][coordinate.col] == (int)GroundType.Ground;
        bool isLeftGround = groundData.data[coordinate.row][coordinate.col - 1] == (int)GroundType.Ground;
        bool isRightGround = groundData.data[coordinate.row][coordinate.col + 1] == (int)GroundType.Ground;

        fieldData.groundDirection = DirectionTypeExtensions.DirectionMarge(isTopGround, isBottomGround, isRightGround, isLeftGround);
    }

    private void SwitchWorldMap()
    {
        isWorldMapShow = !isWorldMapShow; // ワールドマップの表示状態を切り替え
    }
}
