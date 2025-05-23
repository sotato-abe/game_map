using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TargetDatabase : MonoBehaviour
{
    public static TargetDatabase Instance { get; private set; }

    public List<TargetData> targetDataList;
    private Dictionary<TargetType, TargetData> dataDict;

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
        dataDict = new Dictionary<TargetType, TargetData>();

        foreach (var data in targetDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("TargetDatabase: Null TargetData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.targetType))
            {
                Debug.LogWarning($"TargetDatabase: Duplicate entry for {data.targetType} found. Skipping.");
                continue;
            }

            dataDict[data.targetType] = data;
        }
    }

    public TargetData GetData(TargetType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}