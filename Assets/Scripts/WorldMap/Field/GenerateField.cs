using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


//　役割：フィールドの生成を行う
//　マップ生成：座標を受け取ると、WorldMapSystemからフィールドデータを取得
//　フィールドデータからタイルセットを選択しフィールドの描画を行う
//　TODO：ルート生成：フィールドデータ（roadDirection）から出入り口の方角に応じてルートを敷く

public class GenerateField : MonoBehaviour
{
    [SerializeField] GameObject entryPrefab, buildingPrefab, objectItemPrefab; // 地面と壁のプレファブ
    [SerializeField] GameObject fieldCanvas; // フィールドキャンバス
    [SerializeField] List<FloorTileListBase> floorTiles;
    [SerializeField] MapBase mapBase; //マップデータ
    [SerializeField] FieldPlayerController character; //キャラクター
    [SerializeField] WorldMapSystem worldMapSystem;
    public int width;        // マップの幅
    public int height;       // マップの高さ
    DirectionType characterDirection;

    FieldMapGenerator fieldMapGenerator = new FieldMapGenerator();

    Vector2 mapCenterPos;    // マップの中心座標
    float tileSize;          // プレファブのサイズ
    FloorTileListBase tileSet;
    List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを追跡するリスト
    List<Vector2> entryPositions = new List<Vector2>(); // エントリーポイントのリスト

    private FieldData fieldData;

    void Start()
    {
        characterDirection = DirectionType.Top;
        width = mapBase.MapWidth; // マップの幅を取得
        height = mapBase.MapHeight; // マップの高さを取得
        fieldCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height); // フィールドキャンバスのサイズを設定
        fieldData = worldMapSystem.getFieldDataByCoordinate(character.Battler.coordinate);
        fieldMapGenerator.GenarateField(mapBase); // フィールドマップを生成
        renderingTileMap(); // タイルマップを描画
        character.gameObject.transform.position = mapCenterPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            characterDirection = DirectionType.Top;
            ReloadMap(characterDirection, character.Battler.coordinate);
        }
    }

    public void ReloadMap(DirectionType entryDirection, Coordinate playerCoordinate)
    {
        characterDirection = entryDirection;
        fieldData = worldMapSystem.getFieldDataByCoordinate(playerCoordinate);
        ClearMap(); // 現在のマップをクリア
        fieldMapGenerator.GenarateField(mapBase); // フィールドマップを生成
        renderingTileMap(); // タイルマップを描画
    }

    // フィールド用のタイルを描画
    void renderingTileMap()
    {
        tileSet = floorTiles[(int)fieldData.floorType];
        tileSize = tileSet.Floor.bounds.size.x; // タイルサイズを取得
        mapCenterPos = new Vector2(width * tileSize / 2, height * tileSize / 2); // マップの中心座標を計算

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = GetWorldPositionFromTile(x, y); // タイルのワールド座標を計算
                int tileType = fieldMapGenerator.Map[x, y];

                if (tileType == (int)TileType.Base)
                    continue;

                GameObject groundObj = null;
                GameObject obj = null;

                // タイルタイプごとの処理
                if (tileType != (int)TileType.Base && tileType != (int)TileType.Wall && tileType != (int)TileType.Edge)
                {
                    groundObj = CreateTile($"Tile_{x}_{y}", pos, tileSet.Floor, "MapGround", "Ground");
                    if (groundObj != null)
                        groundObj.transform.SetParent(fieldCanvas.transform, false);

                    spawnedObjects.Add(groundObj);
                }
                if (tileType == (int)TileType.Floor)
                    obj = CreateTile($"Tile_{x}_{y}", pos, tileSet.Grass, "MapFloor", "Floor");
                else if (tileType == (int)TileType.Edge)
                    obj = CreateTile($"Tile_{x}_{y}", pos, tileSet.Rock, "MapEdge", "Wall");
                else if (tileType == (int)TileType.Wall)
                    obj = CreateTile($"Tile_{x}_{y}", pos, tileSet.Tree, "MapWall", "Wall");
                else if (tileType == (int)TileType.Entry)
                    obj = InstantiatePrefab(entryPrefab, pos, "MapEntry", "Entry");
                else if (tileType == (int)TileType.Building)
                    obj = InstantiatePrefab(buildingPrefab, pos, "MapBuilding", "Building");
                else if (tileType == (int)TileType.Object)
                    obj = InstantiatePrefab(objectItemPrefab, pos, "ObjectItem", "Object");

                if (obj != null)
                {
                    obj.transform.SetParent(fieldCanvas.transform, false);
                    spawnedObjects.Add(obj);
                }
            }
        }
    }

    GameObject CreateTile(string name, Vector2 position, Sprite sprite, string sortingLayer, string layerName)
    {
        GameObject obj = new GameObject(name);
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingLayerName = sortingLayer;
        obj.layer = LayerMask.NameToLayer(layerName);
        obj.AddComponent<BoxCollider2D>();
        obj.transform.position = position;
        return obj;
    }

    GameObject InstantiatePrefab(GameObject prefab, Vector2 position, string sortingLayer, string layerName)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = sortingLayer;
        obj.layer = LayerMask.NameToLayer(layerName);
        obj.AddComponent<BoxCollider2D>();
        return obj;
    }

    // 座標からワールド座標に変換
    Vector2 GetWorldPositionFromTile(int x, int y)
    {
        return new Vector2(x * tileSize, (height - y) * tileSize); // マップの中心を考慮して座標を計算
    }

    // マップを初期化
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