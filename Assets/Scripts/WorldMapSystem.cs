using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapSystem : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;

    private void Start()
    {
        OutputTileMapData();
    }

    public void OutputTileMapData()
    {
        
    }
}
