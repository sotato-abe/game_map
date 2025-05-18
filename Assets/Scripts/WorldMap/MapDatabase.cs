using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class MapDatabase : MonoBehaviour
{
    public static MapDatabase Instance { get; private set; }

    public List<MapBase> mapDataList;

    private Dictionary<Coordinate, MapBase> coordinateDict;


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
        coordinateDict = new Dictionary<Coordinate, MapBase>();

        foreach (var data in mapDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("MapDatabase: Null MapBase found in list.");
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

    public MapBase GetData(Coordinate coordinate)
    {
        if (coordinateDict != null && coordinateDict.TryGetValue(coordinate, out var data))
        {
            return data;
        }

        return null;
    }

    public List<Coordinate> GetMapCoordinateList()
    {
        List<Coordinate> coordinateList = new List<Coordinate>();

        foreach (var data in mapDataList)
        {
            if (data == null)
            {
                Debug.LogWarning("MapDatabase: Null MapBase found in list.");
                continue;
            }

            coordinateList.Add(data.Coordinate);
        }

        return coordinateList;
    }
}