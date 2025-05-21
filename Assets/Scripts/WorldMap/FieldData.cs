// using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    public Vector2Int coordinate; // 座標
    public MapBase mapBase = null; // マップデータ
    public int mapWidth { get => mapBase != null ? mapBase.MapWidth : 50; } // マップの幅(初期値：50)
    public int mapHeight { get => mapBase != null ? mapBase.MapHeight : 50; } // マップの高さ(初期値：50)
    public int randomFillPercent { get => mapBase != null ? mapBase.RandomFillPercent : 45; } // マップの建蔽率(初期値：45%)
    private FieldType _fieldType;
    public FieldType fieldType { get => mapBase != null ? mapBase.FieldType : _fieldType; set => _fieldType = value; }
    public bool openTop, openLeft, openRight, openBottom;
    public List<Item> items = new List<Item>();
    public List<Battler> enemies = new List<Battler>();
    public List<BuildingBase> Buildings { get => mapBase != null ? mapBase.Buildings : new List<BuildingBase>(); }

    public virtual void Init()
    {
        SetItem();
        SetEnemy();
    }

    private void SetItem()
    {
        if (mapBase != null)
        {
            items.AddRange(mapBase.Items);
        }
        List<Item> fieldItems = FieldBaseDatabase.Instance.GetItemList((FieldType)fieldType);
        items.AddRange(fieldItems);
    }

    private void SetEnemy()
    {
        if (mapBase != null)
        {
            enemies.AddRange(mapBase.Enemies);
        }
        List<Battler> fieldEnemies = FieldBaseDatabase.Instance.GetBattlerList((FieldType)fieldType);
        enemies.AddRange(fieldEnemies);
    }

    public Item GetRandomItem()
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogError("GetRandomItem: itemsリストが空です");
            return null;
        }

        int r = Random.Range(0, items.Count);
        if (items[r] == null)
        {
            Debug.LogError($"GetRandomItem: items[{r}] が null です");
            return null;
        }

        return items[r];
    }

    public Battler GetRundamEnemy()
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogError("GetRandomEnemy: enemiesリストが空です");
            return null;
        }

        int r = Random.Range(0, enemies.Count);
        if (enemies[r] == null)
        {
            Debug.LogError($"GetRandomEnemy: enemies[{r}] が null です");
            return null;
        }

        return enemies[r];
    }
}