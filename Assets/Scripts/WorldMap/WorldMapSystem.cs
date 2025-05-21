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

    /// Jsonのマップデータを読み込む
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

    // 指定座標からフィールドデータを返却する
    public FieldData getFieldDataByCoordinate(Vector2Int targetCoordinate)
    {
        coordinate = targetCoordinate; // 座標を取得

        fieldData = new FieldData(); // フィールドデータを初期化
        fieldData.coordinate = targetCoordinate; // フィールドデータに座標を設定
        fieldData.fieldType = (FieldType)floorData.data[targetCoordinate.y][targetCoordinate.x]; // フィールドデータにタイルセットを設定
        MapBase mapBase = MapDatabase.Instance?.GetDataByCoordinate(coordinate); // フィールドデータを取得
        SetRoadEntryByAroundGround(targetCoordinate); // フィールドデータに出入り口を設定
        renderWorldMap.ChangePlayerCoordinate(coordinate); // TODO : 処理の呼び出し元を変更 ワールドマップのPlayer位置を更新

        return fieldData;
    }

    // 周囲のGroundを取得して、Groundがある方向に出入り口を設定する
    private void SetRoadEntryByAroundGround(Vector2Int targetCoordinate)
    {
        fieldData.openTop = isLand(targetCoordinate.x + 1, targetCoordinate.y); // 上の出入り口
        fieldData.openBottom = isLand(targetCoordinate.x - 1, targetCoordinate.y); // 下の出入り口
        fieldData.openLeft = isLand(targetCoordinate.x, targetCoordinate.y - 1); // 左の出入り口
        fieldData.openRight = isLand(targetCoordinate.x, targetCoordinate.y + 1); // 右の出入り口
    }

    //Roadマップを読み込んで、出入り口を設定（現在未使用中）
    private void SetRoadEntryByRoadMap()
    {
        DirectionType roadType = (DirectionType)roadData.data[coordinate.y][coordinate.x];
        fieldData.openTop = roadType.IsContainCrossDirection(DirectionType.Top); // 上の出入り口
        fieldData.openLeft = roadType.IsContainCrossDirection(DirectionType.Left); // 左の出入り口
        fieldData.openRight = roadType.IsContainCrossDirection(DirectionType.Right); // 右の出入り口
        fieldData.openBottom = roadType.IsContainCrossDirection(DirectionType.Bottom); // 下の出入り口
    }

    private bool isLand(int x, int y)
    {
        int col = x % worldWidth; // x座標をワールドマップの幅で割った余りを取得
        int row = y % worldHeight; // y座標をワールドマップの高さで割った余りを取得
        // 指定座標が海かどうかを確認
        if (floorData.data[row][col] == (int)FieldType.Ocean)
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
