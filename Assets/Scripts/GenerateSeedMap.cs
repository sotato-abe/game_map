using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


//　役割：フィールドの生成を行う
//　仕組み：マップ生成：座標を受け取ると、ワールドマップデータからフィールドタイプを取得、フィールドタイプごとのタイルセットを使用しマップ生成を行う
//　仕組み：ルート生成：座標を受け取ると、ロードマップデータから出入り口のデータを取得、出入り口の方角に応じてルートを敷く

public class GenerateSeedMap : MonoBehaviour
{
    public FieldType fieldType;
    public int width;        // マップの幅
    public int height;       // マップの高さ
    public string seed;      // シード値
    public bool useRandamSeed; // ランダムシードを使うかどうか

    [Range(0, 100)]
    public int randomFillPercent; // 壁の埋め込み率
    [SerializeField] GameObject entryPrefab, buildingPrefab, objectItemPrefab; // 地面と壁のプレファブ
    [SerializeField] List<FieldTileListBase> fieldTiles;
    [SerializeField] MapBase mapBase; //マップデータ
    [SerializeField] GameObject character; //キャラクター
    Vector2 characterPosition;
    int characterDirection;

    int[,] map;             // マップデータ
    int[,] area;             // エリアデータ
    Vector2 mapCenterPos;    // マップの中心座標
    float tileSize;          // プレファブのサイズ
    FieldTileListBase tileSet;

    List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを追跡するリスト
    List<Vector2> entryPositions = new List<Vector2>(); // エントリーポイントのリスト

    void Start()
    {
        characterDirection = 0;
        width = mapBase.MapWidth;
        height = mapBase.MapHeight;
        GenarateMap(); // ゲーム開始時にマップ生成
        character.transform.position = mapCenterPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            characterDirection = 1;
            ReloadMap(characterDirection);
        }
    }

    public void ReloadMap(int entryNum)
    {
        characterDirection = entryNum;
        ClearMap();   // 現在のマップをクリア
        GenarateMap();
    }

    // マップ生成のメソッド
    void GenarateMap()
    {
        if (useRandamSeed)
        {
            seed = Time.time.ToString(); // ランダムシードを現在の時間から生成
        }
        System.Random pseudoRandomMap = new System.Random(seed.GetHashCode()); // シード値に基づいた擬似乱数生成器を作成
        System.Random pseudoRandomArea = new System.Random(seed.GetHashCode() - 1); // シード値に基づいた擬似乱数生成器を作成

        map = new int[width, height];
        area = new int[width, height];
        map = RandamFillMap(map, pseudoRandomMap); // マップにランダムな値を埋め込む
        area = RandamFillMap(area, pseudoRandomArea); // マップにランダムな値を埋め込む

        for (int i = 0; i < 5; i++)
        {
            map = SmoothMap(map);
            area = SmoothMap(area);
        }
        margeArea();
        createBuilding();
        createObjectItem();
        createEntry();
        createWall();
        renderingTileMap();
        renderingObject();
    }

    // マップをランダムに埋めるメソッド
    int[,] RandamFillMap(int[,] field, System.Random seedPercent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    field[x, y] = (int)TileType.Layer;
                }
                else
                {
                    field[x, y] = (seedPercent.Next(0, 100) < randomFillPercent) ? (int)TileType.Layer : (int)TileType.Ground; // ランダムな値で壁か地面を決定
                }
            }
        }

        return field;
    }

    int[,] SmoothMap(int[,] field)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourGroundTiles = GetSurroundingGroundCount(x, y, field);

                if (4 < neighbourGroundTiles)
                {
                    field[x, y] = (int)TileType.Ground;
                }
                else if (neighbourGroundTiles < 4)
                {
                    field[x, y] = (int)TileType.Layer;
                }

            }
        }

        return field;
    }

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
                        groundCount += field[neighbourX, neighbourY];
                    }
                }
            }
        }

        return groundCount;
    }

    void margeArea()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == (int)TileType.Ground && area[x, y] != (int)TileType.Ground)
                {
                    map[x, y] = (int)TileType.Area;
                }
            }
        }
    }

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
            while (map[targetX, targetY] != (int)TileType.Ground && map[targetX, targetY] != (int)TileType.Area && map[targetX, targetY] != (int)TileType.Building); // Ground か area でなければ繰り返し

            map[targetX, targetY] = (int)TileType.Building;
        }
    }

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
                if (map[x, y] != (int)TileType.Layer)
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
            CreateEntryForDirection(0, 1, minY, maxY, false, pseudoRandom, 2); // Left
        if (mapBase.OpenRight)
            CreateEntryForDirection(width - 1, -1, minY, maxY, false, pseudoRandom, 1); // Right
        if (mapBase.OpenBottom)
            CreateEntryForDirection(height - 1, -1, minX, maxX, true, pseudoRandom, 4); // Top
        if (mapBase.OpenTop)
            CreateEntryForDirection(0, 1, minX, maxX, true, pseudoRandom, 3); // Bottom
    }

    // フィールドへのエントリーポイントとルートを作成
    void CreateEntryForDirection(int start, int step, int min, int max, bool isVertical, System.Random pseudoRandom, int direction)
    {
        int entryPoint = pseudoRandom.Next(min, max + 1);
        if (characterDirection == direction)
        {
            if (isVertical)
            {
                character.transform.position = GetWorldPositionFromTile(entryPoint, start + step);
            }
            else
            {
                character.transform.position = GetWorldPositionFromTile(start + step, entryPoint);
            }
        }

        if (isVertical)
        {
            map[entryPoint, start] = (int)TileType.Entry;
            for (int y = start + step; y >= 0 && y < height; y += step)
            {
                if (map[entryPoint, y] == (int)TileType.Layer)
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
                if (map[x, entryPoint] == (int)TileType.Layer)
                    map[x, entryPoint] = (int)TileType.Ground;
                else
                    break;
            }
        }
    }

    void createWall()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == (int)TileType.Layer && CheckSurroundingGround(x, y))
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = (int)TileType.Edge;
                    }
                    else
                    {
                        map[x, y] = (int)TileType.Wall;
                    }
                }
            }
        }
    }

    // 指定座標の周りに地面が存在するかどうかを確認
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
                    // 地面、エリア、建物がある場合にtrueとする
                    if (map[nx, ny] == (int)TileType.Ground || map[nx, ny] == (int)TileType.Area || map[nx, ny] == (int)TileType.Object)
                    {
                        return true; // 隣接する1が見つかった場合
                    }
                }
            }
        }

        return false; // 隣接する1が見つからなかった場合
    }

    //　タイルを敷いていく
    void renderingTileMap()
    {
        // 実際にマップにオブジェクトを配置する処理
        tileSet = fieldTiles[1];
        tileSize = tileSet.Floor.bounds.size.x; // タイルサイズを取得
        Debug.Log($"TileType:{tileSet.Type}");
        mapCenterPos = new Vector2(width * tileSize / 2, height * tileSize / 2); // マップの中心座標を計算

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = GetWorldPositionFromTile(x, y); // タイルのワールド座標を計算
                if (map[x, y] != (int)TileType.Layer && map[x, y] != (int)TileType.Wall && map[x, y] != (int)TileType.Edge)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = tileSet.Floor;
                    renderer.sortingLayerName = "MapGround";
                    obj.layer = LayerMask.NameToLayer("Ground");
                    obj.AddComponent<BoxCollider2D>();
                    obj.transform.position = pos;
                    spawnedObjects.Add(obj);
                }
                if (map[x, y] == (int)TileType.Area)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = tileSet.Grass;
                    renderer.sortingLayerName = "MapArea";
                    obj.layer = LayerMask.NameToLayer("Area");
                    obj.AddComponent<BoxCollider2D>();
                    obj.transform.position = pos;
                    spawnedObjects.Add(obj);
                }
                if (map[x, y] == (int)TileType.Edge)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = tileSet.Rock;
                    renderer.sortingLayerName = "MapEdge";
                    obj.layer = LayerMask.NameToLayer("Wall");
                    obj.AddComponent<BoxCollider2D>();
                    obj.transform.position = pos;
                    spawnedObjects.Add(obj);
                }
                if (map[x, y] == (int)TileType.Wall)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = tileSet.Tree;
                    renderer.sortingLayerName = "MapWall";
                    obj.layer = LayerMask.NameToLayer("Wall");
                    obj.AddComponent<BoxCollider2D>();
                    obj.transform.position = pos;
                    spawnedObjects.Add(obj);
                }
                if (map[x, y] == (int)TileType.Entry)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    obj = Instantiate(entryPrefab, pos, Quaternion.identity); // 入口を生成
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "MapEntry";
                    obj.layer = LayerMask.NameToLayer("Entry");
                    obj.AddComponent<BoxCollider2D>();
                    spawnedObjects.Add(obj);
                }
                if (map[x, y] == (int)TileType.Building)
                {
                    GameObject obj = new GameObject($"Tile_{x}_{y}");
                    obj = Instantiate(buildingPrefab, pos, Quaternion.identity); // 建物を生成
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "MapBuilding";
                    obj.layer = LayerMask.NameToLayer("Building");
                    spawnedObjects.Add(obj);
                }
            }
        }
    }

    void renderingObject()
    {
        // 実際にマップにオブジェクトを配置する処理
        tileSize = tileSet.Floor.bounds.size.x; // タイルサイズを取得
        Debug.Log($"TileType:{tileSet.Type}");
        mapCenterPos = new Vector2(width * tileSize / 2, height * tileSize / 2); // マップの中心座標を計算

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = GetWorldPositionFromTile(x, y); // タイルのワールド座標を計算
                GameObject obj = new GameObject($"Tile_{x}_{y}");
                SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();


                if (map[x, y] == (int)TileType.Object)
                {
                    obj = Instantiate(objectItemPrefab, pos, Quaternion.identity); // アイテムを生成
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "ObjectItem";
                    obj.layer = LayerMask.NameToLayer("Object");
                    spawnedObjects.Add(obj);
                }
            }
        }
    }


    // 座標をタイルからワールド座標に変換するメソッド
    Vector2 GetWorldPositionFromTile(int x, int y)
    {
        return new Vector2(x * tileSize, (height - y) * tileSize); // マップの中心を考慮して座標を計算
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
}

public enum TileType
{
    Layer,
    Ground,
    Area,
    Edge,
    Wall,
    Building,
    Entry,
    Object,
}
