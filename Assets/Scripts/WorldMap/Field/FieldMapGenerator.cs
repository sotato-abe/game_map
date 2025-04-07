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

    public Vector2 topEntoryPosition = Vector2.zero; // 上のエントリーポイント
    public Vector2 bottomEntoryPosition = Vector2.zero; // 下のエントリーポイント
    public Vector2 leftEntoryPosition = Vector2.zero; // 左のエントリーポイント
    public Vector2 rightEntoryPosition = Vector2.zero; // 右のエントリーポイント
    public bool useRandamSeed = false;

    private System.Random pseudoRandomMap;
    private System.Random pseudoRandomFloor;
    DirectionType characterDirection;

    int[,] map;  // マップデータ
    int[,] area; // エリアデータ

    public int[,] Map { get { return map; } } // マップデータの取得
    public int[,] Area { get { return area; } } // エリアデータの取得

    public void GenarateField(MapBase mapBase)
    {
        this.mapBase = mapBase;
        this.width = mapBase.MapWidth;   // マップの幅
        this.height = mapBase.MapHeight; // マップの高さ
        this.randomFillPercent = mapBase.RandomFillPercent; // ランダムで埋め込む確率

        if (useRandamSeed)
        {
            seed = Time.time.ToString(); // ランダムシードを現在の時間から生成
        }
        else
        {
            Debug.Log("Seed: " + mapBase.Coordinate.col.ToString() + mapBase.Coordinate.row.ToString());
            seed = mapBase.Coordinate.col.ToString() + mapBase.Coordinate.row.ToString(); // マップの座標からシードを生成
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
        createObject();
        createEntry();
        createWall();
    }

    // フィールドマップにランダムでグラウンドを追加
    private int[,] RandamFillMap(int[,] field, System.Random seedPercent)
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
    private int[,] SmoothMap(int[,] field)
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
    private int GetSurroundingGroundCount(int gridX, int gridY, int[,] field)
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

    private bool HasCrossGroundCount(int gridX, int gridY, int[,] field)
    {
        if (field[gridX + 1, gridY] == (int)TileType.Ground)
        {
            return true;
        }
        if (field[gridX - 1, gridY] == (int)TileType.Ground)
        {
            return true;
        }
        if (field[gridX, gridY + 1] == (int)TileType.Ground)
        {
            return true;
        }
        if (field[gridX, gridY - 1] == (int)TileType.Ground)
        {
            return true;
        }
        return false;
    }

    // フィールドマップにフロアを追加
    private void margeFloor()
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
    private void createBuilding()
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
    private void createObject()
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
    private void createEntry()
    {
        // 各エントリーポイントの生成処理を共通関数で行う
        topEntoryPosition = TryCreateEntry(mapBase.OpenTop, "top", width, 0, isHorizontal: true);
        bottomEntoryPosition = TryCreateEntry(mapBase.OpenBottom, "bottom", width, height - 1, isHorizontal: true);
        leftEntoryPosition = TryCreateEntry(mapBase.OpenLeft, "left", height, 0, isHorizontal: false);
        rightEntoryPosition = TryCreateEntry(mapBase.OpenRight, "right", height, width - 1, isHorizontal: false);

        CreateRouteForEntry();
    }

    private Vector2 TryCreateEntry(bool isOpen, string direction, int length, int fixedCoord, bool isHorizontal)
    {
        if (!isOpen)
            return new Vector2(-1, -1);

        string entrySeed = seed + direction;
        System.Random prng = new System.Random(entrySeed.GetHashCode());
        int variableCoord = prng.Next(1, length - 1); // 1 ～ length - 2 の範囲でランダム生成

        if (isHorizontal)
        {
            map[variableCoord, fixedCoord] = (int)TileType.Entry;
            return new Vector2(variableCoord, fixedCoord);
        }
        else
        {
            map[fixedCoord, variableCoord] = (int)TileType.Entry;
            return new Vector2(fixedCoord, variableCoord);
        }
    }

    private void CreateRouteForEntry()
    {
        List<Vector2Int> entries = new();
        if (topEntoryPosition.x >= 0) entries.Add(Vector2Int.RoundToInt(topEntoryPosition));
        if (bottomEntoryPosition.x >= 0) entries.Add(Vector2Int.RoundToInt(bottomEntoryPosition));
        if (leftEntoryPosition.x >= 0) entries.Add(Vector2Int.RoundToInt(leftEntoryPosition));
        if (rightEntoryPosition.x >= 0) entries.Add(Vector2Int.RoundToInt(rightEntoryPosition));

        foreach (var entry in entries)
        {
            Vector2Int current = entry;
            int tryCount = 0;

            HashSet<Vector2Int> newPath = new(); // このルート専用のパス記録

            newPath.Add(current);

            bool reachedGoal = false;
            List<Vector2Int> directions = new() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            while (tryCount < 1000)
            {
                tryCount++;

                Shuffle(directions); // ランダム方向

                bool moved = false;

                foreach (var dir in directions)
                {
                    Vector2Int next = current + dir;
                    if (!IsInMap(next)) continue;
                    if (newPath.Contains(next)) continue;

                    // 通行可能なマスなら進む
                    if (map[next.x, next.y] == (int)TileType.Base)
                    {
                        current = next;
                        newPath.Add(current);
                        moved = true;
                        if (HasCrossGroundCount(next.x, next.y, map))
                        {
                            reachedGoal = true;
                        }
                        break;
                    }
                }
                if (reachedGoal || !moved)
                    break;
            }

            foreach (var pos in newPath)
            {
                if (map[pos.x, pos.y] == (int)TileType.Base)
                    map[pos.x, pos.y] = (int)TileType.Floor;
            }

        }
    }

    //　フィールドマップに縁壁とグランドの壁を追加
    private void createWall()
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

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    private bool IsInMap(Vector2Int pos)
    {
        return pos.x >= 1 && pos.x < width - 1 && pos.y >= 1 && pos.y < height - 1;
    }
}
