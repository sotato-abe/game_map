using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class MapDatabase : MonoBehaviour
{
    public static MapDatabase Instance { get; private set; }

    public List<FieldBase> mapDataList;

    private Dictionary<Vector2Int, FieldBase> coordinateDict;


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
        coordinateDict = new Dictionary<Vector2Int, FieldBase>();

        foreach (var data in mapDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("MapDatabase: Null FieldBase found in list.");
                continue;
            }

            if (coordinateDict.ContainsKey(data.Coordinate))
            {
                Debug.LogWarning($"MapDatabase: Duplicate entry for {data.Coordinate} found. Skipping.");
                continue;
            }

            coordinateDict[data.Coordinate] = data;
        }
    }

    public FieldBase GetDataByCoordinate(Vector2Int coordinate)
    {
        if (coordinateDict != null && coordinateDict.TryGetValue(coordinate, out var data))
        {
            return data;
        }

        return null;
    }

    public List<Vector2Int> GetMapCoordinateList()
    {
        List<Vector2Int> coordinateList = new List<Vector2Int>();

        foreach (var data in mapDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("MapDatabase: Null FieldBase found in list.");
                continue;
            }

            coordinateList.Add(data.Coordinate);
        }

        return coordinateList;
    }
}