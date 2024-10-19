using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public Map Map { get; set; }

    public virtual void Setup(Map map)
    {
        Map = map;
    }
}
