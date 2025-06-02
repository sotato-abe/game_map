using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class StatusDatabase : MonoBehaviour
{
    public static StatusDatabase Instance { get; private set; }

    public List<StatusData> statusDataList;
    private Dictionary<StatusType, StatusData> dataDict;

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
        dataDict = new Dictionary<StatusType, StatusData>();

        foreach (var data in statusDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("StatusDatabase: Null StatusData found in list.");
                continue;
            }

            if (dataDict.ContainsKey(data.statusType))
            {
                Debug.LogWarning($"StatusDatabase: Duplicate entry for {data.statusType} found. Skipping.");
                continue;
            }

            dataDict[data.statusType] = data;
        }
    }

    public StatusData GetData(StatusType type)
    {
        if (dataDict != null && dataDict.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}