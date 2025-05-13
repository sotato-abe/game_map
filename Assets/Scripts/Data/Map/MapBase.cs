using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapBase : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string mapName;
    [SerializeField] int level;
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
    [SerializeField] Kiosk kiosk;
    [SerializeField] Cafeteria cafeteria;
    [SerializeField] ArmsShop armsShop;
    [SerializeField] Laboratory laboratory;
    [SerializeField] Hotel hotel;
    [SerializeField] List<Battler> enemies;
    [SerializeField] List<Item> items;

    public int Id { get => id; }
    public string Name { get => mapName; }
    public int Level { get => level; }
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
        get => new Coordinate(coordinateY, coordinateX); // 引数の順番注意！
    }

    public Kiosk Kiosk { get => kiosk; }
    public Cafeteria Cafeteria { get => cafeteria; }
    public ArmsShop ArmsShop { get => armsShop; }
    public Laboratory Laboratory { get => laboratory; }
    public Hotel Hotel { get => hotel; }

    public List<Battler> Enemies { get => enemies; }
    public List<Item> Items { get => items; }
}
