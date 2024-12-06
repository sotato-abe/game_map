using System.Collections.Generic;
using UnityEngine;

public class FieldTileManager : MonoBehaviour
{
    [SerializeField] private List<FieldTileListBase> tileLists;

    public FieldTileListBase GetTileListByID(int id)
    {
        foreach (var tileList in tileLists)
        {
            if ((int)tileList.Type == id) // IDプロパティを使用
            {
                return tileList;
            }
        }
        return null;
    }

public Sprite GetFieldTile(FieldType fieldType, int index)
{
    // fieldType に一致する FieldTileListBase を取得
    var fieldTileList = tileLists.Find(tileList => tileList.Type == fieldType);

    if (fieldTileList == null)
    {
        Debug.LogError($"FieldTileList not found for FieldType: {fieldType}");
        return null;
    }

    // index に対応するタイルを返す
    return index switch
    {
        1 => fieldTileList.Floor,
        2 => fieldTileList.Tree,
        3 => fieldTileList.Grass,
        4 => fieldTileList.Rock,
        _ => throw new System.ArgumentOutOfRangeException($"Invalid index: {index}. Must be between 1 and 4.")
    };
}
}
