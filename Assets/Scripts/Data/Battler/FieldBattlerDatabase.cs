using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class FieldBattlerDatabase : MonoBehaviour
{
    public static FieldBattlerDatabase Instance { get; private set; }

    public List<FieldBattlerData> battlerDataList;
    private Dictionary<FieldType, FieldBattlerData> dataDict;

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
        dataDict = new Dictionary<FieldType, FieldBattlerData>();

        foreach (var data in battlerDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("FieldBattlerDatabase: Null FieldBattlerData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.fieldType))
            {
                Debug.LogWarning($"FieldBattlerDatabase: Duplicate entry for {data.fieldType} found. Skipping.");
                continue;
            }

            dataDict[data.fieldType] = data;
        }
    }

    // FieldTypeに基づいてFieldBattlerDataを返す
    public FieldBattlerData GetData(FieldType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}