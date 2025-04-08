// using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    public Coordinate coordinate; // 座標
    public MapBase mapBase = null; // マップデータ
    public int mapWidth { get => mapBase != null ? mapBase.MapWidth : 50; } // マップの幅(初期値：50)
    public int mapHeight { get => mapBase != null ? mapBase.MapHeight : 50; } // マップの高さ(初期値：50)
    public int randomFillPercent { get => mapBase != null ? mapBase.RandomFillPercent : 45; } // マップの建蔽率(初期値：45%)
    public int building { get => mapBase != null ? mapBase.Building : 3; } // 建物
    public int objectItem { get => mapBase != null ? mapBase.ObjectItem : 3; } // アイテム

    private int _floorType;
    private bool _openTop, _openLeft, _openRight, _openBottom;
    public int floorType { get => mapBase != null ? mapBase.MapTileSet : _floorType; set => _floorType = value; }
    public bool openTop { get => mapBase != null ? mapBase.OpenTop : _openTop; set => _openTop = value; }
    public bool openLeft { get => mapBase != null ? mapBase.OpenLeft : _openLeft; set => _openLeft = value; }
    public bool openRight { get => mapBase != null ? mapBase.OpenRight : _openRight; set => _openRight = value; }
    public bool openBottom { get => mapBase != null ? mapBase.OpenBottom : _openBottom; set => _openBottom = value; }
    public List<Battler> enemies { get => mapBase != null ? mapBase.Enemies : null; }

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
