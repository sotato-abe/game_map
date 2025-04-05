using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldMapGenerator
{
    private int width;
    private int height;
    private string seed;
    private int randomFillPercent; // ランダムで埋め込む確率
    private MapBase mapBase;
    public bool useRandamSeed = true;
    private System.Random pseudoRandomMap;
    private System.Random pseudoRandomFloor;
    DirectionType characterDirection;

    int[,] map;             // マップデータ
    int[,] area;            // エリアデータ

    public int[,] Map { get { return map; } } // マップデータの取得
    public int[,] Area { get { return area; } } // エリアデータの取得

    public void GenarateField(MapBase mapBase)
    {
        this.mapBase = mapBase;
        this.width = mapBase.MapWidth; // マップの幅
        this.height = mapBase.MapHeight; // マップの高さ
        this.randomFillPercent = mapBase.RandomFillPercent; // ランダムで埋め込む確率

        if (useRandamSeed)
        {
            seed = Time.time.ToString(); // ランダムシードを現在の時間から生成
        }
        System.Random pseudoRandomMap = new System.Random(seed.GetHashCode()); // シード値に基づいた擬似乱数生成器を作成
        System.Random pseudoRandomFloor = new System.Random(seed.GetHashCode() - 1); // シード値に基づいた擬似乱数生成器を作成

        map = new int[width, height];
        area = new int[width, height];
        map = RandamFillMap(map, pseudoRandomMap); // マップにランダムな値を埋め込む
        area = RandamFillMap(area, pseudoRandomFloor); // マップにランダムな値を埋め込む

        for (int i = 0; i < 5; i++)
        {
            map = SmoothMap(map);
            area = SmoothMap(area);
        }
        margeFloor();
        createBuilding();
        createObjectItem();
        createEntry();
        createWall();
        // renderingTileMap(); // GenerateField側で呼び出す
    }

    // フィールドマップにランダムでグラウンドを追加
    int[,] RandamFillMap(int[,] field, System.Random seedPercent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    field[x, y] = (int)TileType.Base;
                else
                    field[x, y] = (seedPercent.Next(0, 100) < randomFillPercent) ? (int)TileType.Base : (int)TileType.Ground; // ランダムな値で壁か地面を決定
            }
        }

        return field;
    }

    //　モザイク状のフィールドマップを滑らかにしていく
    int[,] SmoothMap(int[,] field)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourGroundTiles = GetSurroundingGroundCount(x, y, field);

                if (4 < neighbourGroundTiles)
                    field[x, y] = (int)TileType.Ground;
                else if (neighbourGroundTiles < 4)
                    field[x, y] = (int)TileType.Base;
            }
        }

        return field;
    }

    //　指定座標の周囲のグラウンド数を取得
    int GetSurroundingGroundCount(int gridX, int gridY, int[,] field)
    {
        int groundCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        if (field[neighbourX, neighbourY] == (int)TileType.Ground || field[neighbourX, neighbourY] == (int)TileType.Floor || field[neighbourX, neighbourY] == (int)TileType.Object)
                            groundCount++; // 隣接する1が見つかった場合
                    }
                }
            }
        }

        return groundCount;
    }

    // フィールドマップにフロアを追加
    void margeFloor()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == (int)TileType.Ground && area[x, y] != (int)TileType.Ground)
                    map[x, y] = (int)TileType.Floor;
            }
        }
    }

    //　フィールドマップに建物を追加
    void createBuilding()
    {
        int buildingCount = mapBase.Building;
        for (int i = 0; i < buildingCount; i++)
        {
            int targetX, targetY;
            do
            {
                targetX = UnityEngine.Random.Range(1, width - 1);
                targetY = UnityEngine.Random.Range(1, height - 1);
            }
            while (map[targetX, targetY] != (int)TileType.Ground && map[targetX, targetY] != (int)TileType.Floor && map[targetX, targetY] != (int)TileType.Building); // Ground か area でなければ繰り返し

            map[targetX, targetY] = (int)TileType.Building;
        }
    }

    //　フィールドマップにオブジェクトを追加
    void createObjectItem()
    {
        int objectItemCount = mapBase.ObjectItem;

        for (int i = 0; i < objectItemCount; i++)
        {
            int targetX, targetY;
            do
            {
                targetX = UnityEngine.Random.Range(1, width - 1);
                targetY = UnityEngine.Random.Range(1, height - 1);
            }
            while (map[targetX, targetY] == (int)TileType.Building); // Building 以外となるまで繰り返し

            map[targetX, targetY] = (int)TileType.Object;
        }
    }

    //　フィールドマップに出入り口を追加
    void createEntry()
    {
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] != (int)TileType.Base)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }
        // 各方向の入口を生成
        if (mapBase.OpenLeft)
            CreateRouteForEntry(0, 1, minY, maxY, false, pseudoRandom, DirectionType.Left); // Left
        if (mapBase.OpenRight)
            CreateRouteForEntry(width - 1, -1, minY, maxY, false, pseudoRandom, DirectionType.Right); // Right
        if (mapBase.OpenBottom)
            CreateRouteForEntry(height - 1, -1, minX, maxX, true, pseudoRandom, DirectionType.Bottom); // Top
        if (mapBase.OpenTop)
            CreateRouteForEntry(0, 1, minX, maxX, true, pseudoRandom, DirectionType.Top); // Bottom
    }

    // フィールドマップに道路を追加
    void CreateRouteForEntry(int start, int step, int min, int max, bool isVertical, System.Random pseudoRandom, DirectionType direction)
    {
        int entryPoint = pseudoRandom.Next(min, max + 1);

        if (isVertical)
        {
            map[entryPoint, start] = (int)TileType.Entry;
            for (int y = start + step; y >= 0 && y < height; y += step)
            {
                if (map[entryPoint, y] == (int)TileType.Base)
                    map[entryPoint, y] = (int)TileType.Ground;
                else
                    break;
            }
        }
        else
        {
            map[start, entryPoint] = (int)TileType.Entry;
            for (int x = start + step; x >= 0 && x < width; x += step)
            {
                if (map[x, entryPoint] == (int)TileType.Base)
                    map[x, entryPoint] = (int)TileType.Ground;
                else
                    break;
            }
        }
    }

    //　フィールドマップに縁壁とグランドの壁を追加
    void createWall()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == (int)TileType.Base && 0 < GetSurroundingGroundCount(x, y, map))
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        map[x, y] = (int)TileType.Edge;
                    else
                        map[x, y] = (int)TileType.Wall;
                }
            }
        }
    }
}
