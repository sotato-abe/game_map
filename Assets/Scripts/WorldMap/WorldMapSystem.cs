using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json; // Newtonsoft.Jsonを使用

// JSONのマップデータを読み込みフィールドデータを返す
public class WorldMapSystem : MonoBehaviour
{
    TileMapData floorData;
    TileMapData roadData;

    [SerializeField] RenderWorldMap renderWorldMap;

    public FieldData fieldData; // フィールドデータ
    private Vector2Int coordinate; // 座標
    public int worldWidth; // ワールドマップの幅
    public int worldHeight; // ワールドマップの高さ
    private bool isWorldMapShow = false; // ワールドマップの高さ

    void Awake()
    {
        floorData = LoadJsonMapData("FloorTileMapData");
        roadData = LoadJsonMapData("RoadTileMapData");
        worldWidth = floorData.data[0].Length; // ワールドマップの幅を取得
        worldHeight = floorData.data.Count; // ワールドマップの高さを取得
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
    public FieldData getFieldDataByCoordinate(Vector2Int targetCoordinate)
    {
        coordinate = targetCoordinate; // 座標を取得

        fieldData = new FieldData(); // フィールドデータを初期化
        fieldData.coordinate = targetCoordinate; // フィールドデータに座標を設定
        fieldData.fieldType = (FieldType)floorData.data[targetCoordinate.y][targetCoordinate.x]; // フィールドデータにタイルセットを設定
        fieldData.groundDirection = SeachAroundGround();
        MapBase mapBase = MapDatabase.Instance?.GetData(coordinate); // フィールドデータを取得

        // mapBaseが見つかった場合
        if (mapBase != null)
        {
            fieldData.mapBase = mapBase;
            fieldData.fieldType = fieldData.mapBase.FieldType;
        }
        else
        {
            // Debug.LogWarning($"該当する MapBase が見つかりませんでした。座標: row={coordinate.y}, col={coordinate.x}");
            fieldData.mapBase = null; // フィールドデータにnullを設定
        }
        SetRoadEntry(); // フィールドデータに出入り口を設定
        renderWorldMap.ChangePlayerCoordinate(coordinate); // TODO : 処理の呼び出し元を変更 ワールドマップのPlayer位置を更新

        return fieldData;
    }

    private void SetMapTileSet()
    {
        FieldType newFieldType = FieldType.Default; // フィールドタイプを初期化
        if (fieldData.mapBase != null)
        {
            newFieldType = fieldData.mapBase.FieldType;
        }
        else
        {
            newFieldType = (FieldType)floorData.data[coordinate.y][coordinate.x];
        }
        fieldData.fieldType = newFieldType; // マップのタイルセットを取得
    }

    private void SetRoadEntry()
    {
        DirectionType roadType = (DirectionType)roadData.data[coordinate.y][coordinate.x];
        fieldData.openTop = roadType.IsContainCrossDirection(DirectionType.Top); // 上の出入り口
        fieldData.openLeft = roadType.IsContainCrossDirection(DirectionType.Left); // 左の出入り口
        fieldData.openRight = roadType.IsContainCrossDirection(DirectionType.Right); // 右の出入り口
        fieldData.openBottom = roadType.IsContainCrossDirection(DirectionType.Bottom); // 下の出入り口
    }

    // 周囲のGroundを取得する
    private DirectionType SeachAroundGround()
    {
        bool isTopGround = isLand(coordinate.x + 1, coordinate.y);
        bool isBottomGround = isLand(coordinate.x - 1, coordinate.y);
        bool isLeftGround = isLand(coordinate.x, coordinate.y - 1);
        bool isRightGround = isLand(coordinate.x, coordinate.y + 1);

        return DirectionTypeExtensions.DirectionMarge(isTopGround, isBottomGround, isRightGround, isLeftGround);
    }

    private bool isLand(int col, int row)
    {
        // 指定座標が海かどうかを確認
        if (floorData.data[row][col] == (int)FieldType.Sea)
        {
            return false; // 海の場合はfalseを返す
        }
        else if (floorData.data[row][col] == (int)FieldType.Ocean)
        {
            return false; // 海の場合はfalseを返す
        }
        return true; // 大地の場合はtrueを返す
    }

    private void SwitchWorldMap()
    {
        isWorldMapShow = !isWorldMapShow; // ワールドマップの表示状態を切り替え
    }
}
