using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//　役割：フィールドの生成を行う
//　マップ生成：座標を受け取ると、WorldMapSystemからフィールドデータを取得
//　フィールドデータからタイルセットを選択しフィールドの描画を行う
//　TODO：ルート生成：フィールドデータ（roadDirection）から出入り口の方角に応じてルートを敷く

public class FieldSystem : MonoBehaviour
{
    public UnityAction OnReserve;
    public UnityAction OnEncount; // エンカウントイベント
    FieldMapGenerator fieldMapGenerator = new FieldMapGenerator();

    [SerializeField] GameObject entryPrefab, buildingPrefab, objectItemPrefab; // 地面と壁のプレファブ
    [SerializeField] GameObject fieldCanvas; // フィールドキャンバス
    [SerializeField] List<FloorTileListBase> floorTiles;
    [SerializeField] FieldPlayerController fieldPlayerController; //キャラクター
    [SerializeField] WorldMapSystem worldMapSystem;
    DirectionType playerDirection = DirectionType.Top; // キャラクターの方向

    public PlayerBattler playerBattler;

    float tileSize; // プレファブのサイズ
    FloorTileListBase tileSet;
    List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを追跡するリスト

    private FieldData fieldData;

    void Start()
    {

    }

    public void Setup(PlayerBattler battler)
    {

        playerBattler = battler; // プレイヤーのバトラーを設定
        fieldPlayerController.playerBattler = playerBattler; // プレイヤーのバトラーを設定
        fieldPlayerController.OnReserve += ReserveStart;
        fieldPlayerController.OnEncount += Encount;
        fieldPlayerController.ChangeField += ReloadMap;

        fieldData = worldMapSystem.getFieldDataByCoordinate(playerBattler.coordinate);
        fieldCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(fieldData.mapWidth, fieldData.mapHeight); // フィールドキャンバスのサイズを設定        
        fieldMapGenerator.GenarateField(fieldData); // フィールドマップを生成
        renderingTileMap(); // タイルマップを描画
        Vector2 centerPotision = new Vector2(fieldData.mapWidth * tileSize / 2, fieldData.mapHeight * tileSize / 2);

        fieldPlayerController.gameObject.transform.position = centerPotision;
    }

    public void PlayerMovableSwitch(bool canMove)
    {
        fieldPlayerController.SetMoveFlg(canMove); // プレイヤーの移動フラグを設定
    }

    public void ReserveStart()
    {
        Debug.Log("Encount");
        OnReserve?.Invoke();
    }

    public void Encount()
    {
        Debug.Log("Encount");
        OnEncount?.Invoke();
    }

    public void ReloadMap(DirectionType outDirection)
    {
        DirectionType entryDirection = outDirection.GetOppositeDirection();
        if (outDirection == DirectionType.Top)
            playerBattler.coordinate.row = playerBattler.coordinate.row - 1;
        if (outDirection == DirectionType.Bottom)
            playerBattler.coordinate.row = playerBattler.coordinate.row + 1;
        if (outDirection == DirectionType.Right)
            playerBattler.coordinate.col = playerBattler.coordinate.col + 1;
        if (outDirection == DirectionType.Left)
            playerBattler.coordinate.col = playerBattler.coordinate.col - 1;

        playerDirection = entryDirection;
        fieldData = worldMapSystem.getFieldDataByCoordinate(playerBattler.coordinate); // フィールドデータを取得
        ClearMap(); // 現在のマップをクリア

        fieldMapGenerator.GenarateField(fieldData); // フィールドマップを生成
        renderingTileMap(); // タイルマップを描画
        ResetCharacterPosition();
    }

    // フィールド用のタイルを描画
    void renderingTileMap()
    {
        tileSet = floorTiles[(int)fieldData.floorType];
        tileSize = tileSet.Floor.bounds.size.x; // タイルサイズを取得

        for (int x = 0; x < fieldData.mapWidth; x++)
        {
            for (int y = 0; y < fieldData.mapHeight; y++)
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

    private void ResetCharacterPosition()
    {
        Vector2 position = Vector2.zero; // 初期位置をゼロに設定
        switch (playerDirection)
        {
            case DirectionType.Top:
                position = new Vector2(fieldMapGenerator.topEntoryPosition.x, fieldMapGenerator.topEntoryPosition.y + 1); // 上の出入り口の位置を取得
                break;
            case DirectionType.Bottom:
                position = new Vector2(fieldMapGenerator.bottomEntoryPosition.x, fieldMapGenerator.bottomEntoryPosition.y - 1); // 上の出入り口の位置を取得
                break;
            case DirectionType.Right:
                position = new Vector2(fieldMapGenerator.rightEntoryPosition.x - 1, fieldMapGenerator.rightEntoryPosition.y); // 上の出入り口の位置を取得
                break;
            case DirectionType.Left:
                position = new Vector2(fieldMapGenerator.leftEntoryPosition.x + 1, fieldMapGenerator.leftEntoryPosition.y); // 上の出入り口の位置を取得
                break;
        }
        int x = (int)position.x;
        int y = (int)position.y;
        fieldPlayerController.gameObject.transform.position = GetWorldPositionFromTile(x, y);
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

    // 座標からワールド座標に変換
    Vector2 GetWorldPositionFromTile(int x, int y)
    {
        return new Vector2(x * tileSize, (fieldData.mapHeight - y) * tileSize); // マップの中心を考慮して座標を計算
    }
}