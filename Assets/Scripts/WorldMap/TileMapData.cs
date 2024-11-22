using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileMapData
{
    public List<int[]> data;  // 1次元配列のリストでタイルデータを保持
    public int rows;          // 行数
    public int cols;          // 列数

    public TileMapData(List<int[]> listData)
    {
        data = listData;      // タイルデータの初期化
        rows = listData.Count;  // 行数の設定
        cols = listData[0].Length;  // 列数の設定
    }

    public string GetJson()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
            Debug.Log($"JSON出力: {json}");
            return json;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON変換に失敗しました: {e.Message}");
            return string.Empty;
        }
    }
}
