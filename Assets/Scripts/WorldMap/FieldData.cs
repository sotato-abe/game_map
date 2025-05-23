// using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    public Vector2Int coordinate; // 座標
    public MapBase mapBase = null; // マップデータ
    public DirectionType groundDirection = DirectionType.None; // 進行方向
    public int mapWidth { get => mapBase != null ? mapBase.MapWidth : 50; } // マップの幅(初期値：50)
    public int mapHeight { get => mapBase != null ? mapBase.MapHeight : 50; } // マップの高さ(初期値：50)
    public int randomFillPercent { get => mapBase != null ? mapBase.RandomFillPercent : 45; } // マップの建蔽率(初期値：45%)
    private FieldType _fieldType;
    private bool _openTop, _openLeft, _openRight, _openBottom;
    public FieldType fieldType { get => mapBase != null ? mapBase.FieldType : _fieldType; set => _fieldType = value; }
    public bool openTop { get => mapBase != null ? mapBase.OpenTop : _openTop; set => _openTop = value; }
    public bool openLeft { get => mapBase != null ? mapBase.OpenLeft : _openLeft; set => _openLeft = value; }
    public bool openRight { get => mapBase != null ? mapBase.OpenRight : _openRight; set => _openRight = value; }
    public bool openBottom { get => mapBase != null ? mapBase.OpenBottom : _openBottom; set => _openBottom = value; }

    public Kiosk kiosk { get => mapBase != null ? mapBase.Kiosk : null; }
    public Cafeteria cafeteria { get => mapBase != null ? mapBase.Cafeteria : null; }
    public ArmsShop armsShop { get => mapBase != null ? mapBase.ArmsShop : null; }
    public Laboratory laboratory { get => mapBase != null ? mapBase.Laboratory : null; }
    public Hotel hotel { get => mapBase != null ? mapBase.Hotel : null; }
    public List<Battler> enemies = new List<Battler>();
    public List<Item> items = new List<Item>(); // アイテムリスト

    public virtual void Init()
    {
        if (mapBase != null)
        {
            items = new List<Item>(mapBase.Items);
        }
        SetEnemy();
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
}