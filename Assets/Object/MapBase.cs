using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] new int level;
    [SerializeField] new bool openTop;
    [SerializeField] new bool openLeft;
    [SerializeField] new bool openRight;
    [SerializeField] new bool openBottom;
    [SerializeField] new int mapTileSet;
    [SerializeField] new int mapHeight;
    [SerializeField] new int mapWidth;

    public string Name { get => name; }
    public int Level { get => level; }
    public bool OpenTop { get => openTop; }
    public bool OpenLeft { get => openLeft; }
    public bool OpenRight { get => openRight; }
    public bool OpenBottom { get => openBottom; }
    public int MapTileSet { get => mapTileSet; }
    public int MapHeight { get => mapHeight; }
    public int MapWidth { get => mapWidth; }
}
