using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FieldBase : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string mapName;
    [SerializeField] int level;
    [SerializeField] FieldType fieldType;
    [SerializeField] int mapHeight = 50;
    [SerializeField] int mapWidth = 50;
    [SerializeField, TextArea] string description;
    [SerializeField] int randomFillPercent = 45; // マップの建蔽率
    [SerializeField] Vector2Int coordinate;
    [SerializeField] List<Item> items;
    [SerializeField] List<Battler> enemies;
    [SerializeField] List<BuildingBase> buildings;

    public int Id { get => id; }
    public string Name { get => mapName; }
    public int Level { get => level; }
    public FieldType FieldType { get => fieldType; }
    public int MapHeight { get => mapHeight; }
    public int MapWidth { get => mapWidth; }
    public string Description { get => description; }
    public int RandomFillPercent { get => randomFillPercent; }
    public Vector2Int Coordinate { get => coordinate; }

    public List<Battler> Enemies { get => enemies; }
    public List<Item> Items { get => items; }
    public List<BuildingBase> Buildings { get => buildings; }
}
