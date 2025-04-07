using System.Collections.Generic;

public class FieldData
{
    public Coordinate coordinate; // 座標
    public MapBase spot = null; // マップデータ
    public int mapWidth { get => spot != null ? spot.MapWidth : 50; } // マップの幅(初期値：50)
    public int mapHeight { get => spot != null ? spot.MapHeight : 50; } // マップの高さ(初期値：50)
    public int randomFillPercent { get => spot != null ? spot.RandomFillPercent : 45; } // マップの建蔽率(初期値：45%)
    public int building { get => spot != null ? spot.Building : 3; } // 建物
    public int objectItem { get => spot != null ? spot.ObjectItem : 3; } // アイテム

    private int _floorType;
    private bool _openTop, _openLeft, _openRight, _openBottom;
    public int floorType { get => spot != null ? spot.MapTileSet : _floorType; set => _floorType = value; }
    public bool openTop { get => spot != null ? spot.OpenTop : _openTop; set => _openTop = value; }
    public bool openLeft { get => spot != null ? spot.OpenLeft : _openLeft; set => _openLeft = value; }
    public bool openRight { get => spot != null ? spot.OpenRight : _openRight; set => _openRight = value; }
    public bool openBottom { get => spot != null ? spot.OpenBottom : _openBottom; set => _openBottom = value; }

}
