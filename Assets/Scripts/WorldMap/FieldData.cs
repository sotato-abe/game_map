// using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    public Vector2Int coordinate; // 座標
    public FieldBase fieldBase = null; // マップデータ
    public int mapWidth { get => fieldBase != null ? fieldBase.MapWidth : 50; } // マップの幅(初期値：50)
    public int mapHeight { get => fieldBase != null ? fieldBase.MapHeight : 50; } // マップの高さ(初期値：50)
    public int randomFillPercent { get => fieldBase != null ? fieldBase.RandomFillPercent : 45; } // マップの建蔽率(初期値：45%)
    private FieldType _fieldType;
    public FieldType fieldType { get => fieldBase != null ? fieldBase.FieldType : _fieldType; set => _fieldType = value; }
    public bool openTop, openLeft, openRight, openBottom;
    public List<Item> items = new List<Item>();
    public List<Battler> enemies = new List<Battler>();
    public List<BuildingBase> Buildings { get => fieldBase != null ? fieldBase.Buildings : new List<BuildingBase>(); }

    public virtual void Init()
    {
        SetItem();
        SetEnemy();
    }

    private void SetItem()
    {
        if (fieldBase != null)
        {
            items.AddRange(fieldBase.Items);
        }
        List<Item> fieldItems = FieldBaseDatabase.Instance.GetItemList((FieldType)fieldType);
        items.AddRange(fieldItems);
    }

    private void SetEnemy()
    {
        if (fieldBase != null)
        {
            enemies.AddRange(fieldBase.Enemies);
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