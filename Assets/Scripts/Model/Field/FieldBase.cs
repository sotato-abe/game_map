using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFieldBase", menuName = "Field/FieldBase")]
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
    [SerializeField] List<Consumable> consumables;
    [SerializeField] List<Equipment> equipments;
    [SerializeField] List<Treasure> treasures;
    [SerializeField] List<Battler> enemies;
    [SerializeField] List<BuildingBase> buildings;
    [SerializeField] List<BattlerGroup> enemyGroups = new List<BattlerGroup>();

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
    public List<Consumable> Consumables { get => consumables; }
    public List<Equipment> Equipments { get => equipments; }
    public List<Treasure> Treasures { get => treasures; }
    public List<BuildingBase> Buildings { get => buildings; }
    public List<BattlerGroup> EnemyGroups { get => enemyGroups; }
}
