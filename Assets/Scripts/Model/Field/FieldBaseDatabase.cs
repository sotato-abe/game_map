using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class FieldBaseDatabase : MonoBehaviour
{
    public static FieldBaseDatabase Instance { get; private set; }

    public List<FieldBaseData> fieldBaseDataList;
    private Dictionary<FieldType, FieldBaseData> dataDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        dataDict = new Dictionary<FieldType, FieldBaseData>();

        foreach (var data in fieldBaseDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("FieldBaseDatabase: Null FieldBaseData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.fieldType))
            {
                Debug.LogWarning($"FieldBaseDatabase: Duplicate entry for {data.fieldType} found. Skipping.");
                continue;
            }

            dataDict[data.fieldType] = data;
        }
    }

    // FieldTypeに基づいてFieldBaseDataを返す
    public FieldBaseData GetData(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }

    public List<Item> GetItemList(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data.itemList;
        }

        return null;
    }

    public List<Battler> GetBattlerList(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data.battlerList;
        }

        return null;
    }

    public FieldTileListBase GetTileList(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data.tileList;
        }

        return null;
    }
}