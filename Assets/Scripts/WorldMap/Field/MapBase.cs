using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int level;
    [SerializeField] int building;
    [SerializeField] int objectItem;
    [SerializeField] bool openTop;
    [SerializeField] bool openLeft;
    [SerializeField] bool openRight;
    [SerializeField] bool openBottom;
    [SerializeField] int mapTileSet;
    [SerializeField] int mapHeight;
    [SerializeField] int mapWidth;
    [SerializeField] string description;
    [SerializeField] int randomFillPercent = 45; // マップの建蔽率
    [SerializeField] int coordinateX;
    [SerializeField] int coordinateY;
    public string Name { get => name; }
    public int Level { get => level; }
    public int Building { get => building; }
    public int ObjectItem { get => objectItem; }
    public bool OpenTop { get => openTop; }
    public bool OpenLeft { get => openLeft; }
    public bool OpenRight { get => openRight; }
    public bool OpenBottom { get => openBottom; }
    public int MapTileSet { get => mapTileSet; }
    public int MapHeight { get => mapHeight; }
    public int MapWidth { get => mapWidth; }
    public string Description { get => description; }
    public int RandomFillPercent { get => randomFillPercent; }

    public Coordinate Coordinate
    {
        get => new Coordinate { col = coordinateX, row = coordinateY };
    }
}
