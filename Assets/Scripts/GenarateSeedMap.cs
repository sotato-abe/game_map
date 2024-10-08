using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GenarateSeedMap : MonoBehaviour
{
    public int width;        // マップの幅
    public int height;       // マップの高さ

    public string seed;      // シード値
    public bool useRandamSeed; // ランダムシードを使うかどうか

    [Range(0, 100)]
    public int randomFillPercent; // 壁のランダムな埋め込み率
    [SerializeField] GameObject groundPrefab1, wallPrefab1; // 地面と壁のプレファブ

    int[,] map;             // マップデータ
    Vector2 mapCenterPos;    // マップの中心座標
    float tileSize;          // プレファブのサイズ

    List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを追跡するリスト

    void Start()
    {
        GenarateMap(); // ゲーム開始時にマップ生成
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("reload");

            ClearMap();   // 現在のマップをクリア
            GenarateMap();
        }
    }

    // マップ生成のメソッド
    void GenarateMap()
    {
        map = new int[width, height];
        RandamFillMap(); // マップにランダムな値を埋め込む

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        createWall();

        // 実際にマップにオブジェクトを配置する処理
        tileSize = groundPrefab1.GetComponent<SpriteRenderer>().bounds.size.x; // タイルサイズを取得
        mapCenterPos = new Vector2(width * tileSize / 2, height * tileSize / 2); // マップの中心座標を計算

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = GetWorldPositionFromTile(x, y); // タイルのワールド座標を計算
                GameObject obj;
                if (map[x, y] == (int)TileType.Ground)
                {
                    obj = Instantiate(groundPrefab1, pos, Quaternion.identity); // 地面を生成
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "MapGround"; // 地面用のソーティングレイヤーを設定
                    obj.GetComponent<SpriteRenderer>().sortingOrder = 1; // レイヤー内での描画順を設定
                    spawnedObjects.Add(obj); // 生成されたオブジェクトをリストに追加
                }
                else if (map[x, y] == (int)TileType.Wall)
                {
                    obj = Instantiate(wallPrefab1, pos, Quaternion.identity); // 壁を生成
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "MapWall"; // 壁用のソーティングレイヤーを設定
                    obj.GetComponent<SpriteRenderer>().sortingOrder = 2; // レイヤー内での描画順を設定
                    spawnedObjects.Add(obj); // 生成されたオブジェクトをリストに追加
                }
            }
        }
    }

    // マップをランダムに埋めるメソッド
    void RandamFillMap()
    {
        if (useRandamSeed)
        {
            seed = Time.time.ToString(); // ランダムシードを現在の時間から生成
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); // シード値に基づいた擬似乱数生成器を作成

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = (int)TileType.Layer;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? (int)TileType.Layer : (int)TileType.Ground; // ランダムな値で壁か地面を決定
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourGroundTiles = GetSurroundingGroundCount(x, y);

                if (4 < neighbourGroundTiles)
                {
                    map[x, y] = (int)TileType.Ground;
                }
                else if (neighbourGroundTiles < 4)
                {
                    map[x, y] = (int)TileType.Layer;
                }

            }
        }
    }

    int GetSurroundingGroundCount(int gridX, int gridY)
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
                        groundCount += map[neighbourX, neighbourY];
                    }
                }
            }
        }

        return groundCount;
    }

    // マップのクリアメソッド
    void ClearMap()
    {
        // 生成されたオブジェクトを全て削除
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // リストをクリア
    }

    // 座標をタイルからワールド座標に変換するメソッド
    Vector2 GetWorldPositionFromTile(int x, int y)
    {
        return new Vector2(x * tileSize, (height - y) * tileSize) - mapCenterPos; // マップの中心を考慮して座標を計算
    }

    void createWall()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == (int)TileType.Layer && CheckSurroundingGround(x, y))
                {
                    map[x, y] = (int)TileType.Wall; // 隣に1がある0の位置を2に変更
                }
            }
        }
    }

    bool CheckSurroundingGround(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // 自分自身の位置は無視する
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                // マップの範囲外を除外する
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (map[nx, ny] == (int)TileType.Ground)
                    {
                        return true; // 隣接する1が見つかった場合
                    }
                }
            }
        }

        return false; // 隣接する1が見つからなかった場合
    }
}

public enum TileType
{
    Layer,
    Ground,
    Wall
}
