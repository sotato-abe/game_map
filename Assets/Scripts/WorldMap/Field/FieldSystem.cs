using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//　役割：フィールドの生成を行う
//　マップ生成：座標を受け取ると、WorldMapSystemからフィールドデータを取得
//　フィールドデータからタイルセットを選択しフィールドの描画を行う

public class FieldSystem : MonoBehaviour
{
    public UnityAction OnReserve; // リザーブイベント
    public UnityAction OnEncount; // エンカウントイベント

    FieldMapGenerator fieldMapGenerator = new FieldMapGenerator();

    [SerializeField] GameObject entryPrefab, buildingPrefab, objectItemPrefab; // タイルのプレファブ
    [SerializeField] GameObject kioskPrefab, cafeteriaPrefab, armsShopPrefab, laboratoryPrefab, hotelPrefab; // 建物のプレファブ
    [SerializeField] GameObject fieldCanvas; // フィールドキャンバス
    [SerializeField] List<FieldTileListBase> FieldTileLists;
    [SerializeField] FieldPlayer fieldPlayer; //キャラクター
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] WorldMapSystem worldMapSystem;
    [SerializeField] HitTargetPin hitTargetPin;
    [SerializeField] MessagePanel messagePanel;

    DirectionType playerDirection = DirectionType.None; // キャラクターの方向

    public PlayerBattler playerBattler;

    float tileSize; // プレファブのサイズ
    FieldTileListBase tileSet;
    List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを追跡するリスト

    private FieldData fieldData;
    private BuildingBase currentBuildingBase;

    void Start()
    {
        fieldPlayer.OnReserve += ReserveStart;
        fieldPlayer.OnEncount += Encount;
        fieldPlayer.OnGetItem += GetItem;
        fieldPlayer.ChangeField += ReloadMap;
        fieldPlayer.ResetFieldPanel += ResetFieldInfoPanel;
        fieldPlayer.EntryBuilding += EntryBuilding;
    }

    public void Setup(PlayerBattler battler)
    {
        playerBattler = battler; // プレイヤーのバトラーを設定

        SetUpField();
        fieldMapGenerator.GenarateField(fieldData); // フィールドマップを生成
        renderingTileMap(); // タイルマップを描画
        Vector2 centerPotision = new Vector2(fieldData.mapWidth * tileSize / 2, fieldData.mapHeight * tileSize / 2);
        SetUpFieldPlayerMapSize();
        ResetCharacterPosition();
    }

    private void SetUpField()
    {
        fieldData = worldMapSystem.getFieldDataByCoordinate(playerBattler.coordinate);
        fieldData.Init(); // フィールドデータの初期化
        ResetFieldInfoPanel();
        fieldPlayer.canEncount = fieldData.enemies.Count > 0; // エンカウントフラグを設定
        fieldCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(fieldData.mapWidth, fieldData.mapHeight); // フィールドキャンバスのサイズを設定        
    }

    public void ReserveStart()
    {
        messagePanel.AddMessage(MessageIconType.Bag, $"バックをひらいた");
        OnReserve?.Invoke();
    }

    public void Encount()
    {
        OnEncount?.Invoke();
    }

    public void EntryBuilding(BuildingType type)
    {
        // 現在地のタイルタイプを取得
        BuildingBase building = ScriptableObject.CreateInstance<BuildingBase>();
        switch (type)
        {
            case BuildingType.Kiosk:
                building = fieldData.kiosk.Base; // キオスクの情報を取得
                break;
            case BuildingType.Cafeteria:
                building = fieldData.cafeteria.Base; // カフェテリアの情報を取得
                break;
            case BuildingType.ArmShop:
                building = fieldData.armsShop.Base; // 武器屋の情報を取得
                break;
            case BuildingType.Laboratory:
                building = fieldData.laboratory.Base; // 研究所の情報を取得
                break;
            case BuildingType.Hotel:
                building = fieldData.hotel.Base; // ホテルの情報を取得
                break;
        }
        if (currentBuildingBase != building)
        {
            fieldInfoPanel.gameObject.SetActive(true);
            fieldInfoPanel.SetupBuilding(building);
            messagePanel.AddMessage(MessageIconType.Building, $"{building.Name}");
            currentBuildingBase = building; // 現在の建物を更新
        }
        fieldPlayer.SetMoveFlg(true); // 移動フラグをオンにする
    }


    public void GetItem()
    {
        Item item = fieldData.GetRandomItem(); // ランダムなアイテムを取得
        int getProbability = 20;
        if (item == null || Random.Range(0, 100) < getProbability)
        {
            messagePanel.AddMessage(MessageIconType.Item, $"ボックスは空だった!!");
        }
        else
        {
            messagePanel.AddMessage(MessageIconType.Item, $"{item.Base.Name} ゲット!!");
            playerBattler.AddItem(item); // プレイヤーのインベントリに追加
            fieldData.items.Remove(item); // フィールドからアイテムを削除
        }
        Vector3 pos = fieldPlayer.transform.position;
        Collider2D hit = Physics2D.OverlapCircle(pos, 0.3f, LayerMask.GetMask("Object")); // 近くのObjectレイヤー探す
        if (hit != null)
        {
            Destroy(hit.gameObject); // ヒットしたオブジェクトを削除
        }
        fieldPlayer.SetMoveFlg(true);
    }

    public void ReloadMap(DirectionType outDirection)
    {
        messagePanel.AddMessage(MessageIconType.Field, $"フィールドを移動した!!");
        ClearMap(); // マップクリア
        playerDirection = outDirection.GetOppositeDirection();

        // 移動処理
        Vector2Int coord = playerBattler.coordinate; // プレイヤーの座標を取得

        switch (playerDirection)
        {
            case DirectionType.Top:
                coord.y--;
                fieldData.openTop = true;
                break;
            case DirectionType.Bottom:
                coord.y++;
                fieldData.openBottom = true;
                break;
            case DirectionType.Left:
                coord.x++;
                fieldData.openLeft = true;
                break;
            case DirectionType.Right:
                coord.x--;
                fieldData.openRight = true;
                break;
        }

        // 範囲補正（ループマップ的に回る）
        int maxCol = worldMapSystem.worldHeight;
        int maxRow = worldMapSystem.worldWidth;
        coord.x = (coord.x + maxRow) % maxRow;
        coord.y = (coord.y + maxCol) % maxCol;
        playerBattler.coordinate.x = coord.x;
        playerBattler.coordinate.y = coord.y; // プレイヤーの座標を更新

        // フィールドデータ取得＆Canvasサイズ変更
        SetUpField();

        // 出入り口が1つ以下なら全部開ける
        int openCount = (fieldData.openTop ? 1 : 0) + (fieldData.openBottom ? 1 : 0) + (fieldData.openLeft ? 1 : 0) + (fieldData.openRight ? 1 : 0);
        if (openCount <= 1)
        {
            fieldData.openTop = fieldData.openBottom = fieldData.openLeft = fieldData.openRight = true;
        }

        // フィールド生成＆描画
        fieldMapGenerator.GenarateField(fieldData);
        renderingTileMap();
        ResetCharacterPosition();
        SetUpFieldPlayerMapSize();
    }

    // フィールド用のタイルを描画
    void renderingTileMap()
    {
        tileSet = FieldTileLists[(int)fieldData.fieldType];
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
                // TODO buildingがアイコンを持っている場合アイコンを使用
                if (tileType != (int)TileType.Base && tileType != (int)TileType.Wall && tileType != (int)TileType.Edge)
                {
                    groundObj = CreateTile($"Tile_{x}_{y}", tileSet.Floor, pos, "MapGround", "Ground");
                    if (groundObj != null)
                        groundObj.transform.SetParent(fieldCanvas.transform, false);

                    spawnedObjects.Add(groundObj);
                }
                if (tileType == (int)TileType.Floor)
                    obj = CreateTile($"Tile_{x}_{y}", tileSet.Grass, pos, "MapFloor", "Floor");
                else if (tileType == (int)TileType.Edge)
                    obj = CreateTile($"Tile_{x}_{y}", tileSet.Rock, pos, "MapEdge", "Wall");
                else if (tileType == (int)TileType.Wall)
                    obj = CreateTile($"Tile_{x}_{y}", tileSet.Tree, pos, "MapWall", "Wall");
                else if (tileType == (int)TileType.Entry)
                    obj = InstantiatePrefab(entryPrefab, pos, "MapEntry", "Entry");
                else if (tileType == (int)TileType.Object)
                    obj = InstantiatePrefab(objectItemPrefab, pos, "ObjectItem", "Object");
                else if (tileType == (int)TileType.Kiosk)
                    obj = InstantiateBuildingPrefab(kioskPrefab, pos, "MapBuilding", "Building", BuildingType.Kiosk);
                else if (tileType == (int)TileType.Cafeteria)
                    obj = InstantiateBuildingPrefab(cafeteriaPrefab, pos, "MapBuilding", "Building", BuildingType.Cafeteria);
                else if (tileType == (int)TileType.ArmsShop)
                    obj = InstantiateBuildingPrefab(armsShopPrefab, pos, "MapBuilding", "Building", BuildingType.ArmShop);
                else if (tileType == (int)TileType.Laboratory)
                    obj = InstantiateBuildingPrefab(laboratoryPrefab, pos, "MapBuilding", "Building", BuildingType.Laboratory);
                else if (tileType == (int)TileType.Hotel)
                    obj = InstantiateBuildingPrefab(hotelPrefab, pos, "MapBuilding", "Building", BuildingType.Hotel);

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
            case DirectionType.None:
                position = getRundomFloorPosition();
                break;
        }
        fieldPlayer.gameObject.transform.position = GetCharacterPositionFromCoordinate((int)position.x, (int)position.y);
    }

    private Vector2 getRundomFloorPosition()
    {
        // フィールドのランダムな位置を取得
        int x = UnityEngine.Random.Range(0, fieldData.mapWidth);
        int y = UnityEngine.Random.Range(0, fieldData.mapHeight);
        while (fieldMapGenerator.Map[x, y] != (int)TileType.Floor)
        {
            x = UnityEngine.Random.Range(0, fieldData.mapWidth);
            y = UnityEngine.Random.Range(0, fieldData.mapHeight);
        }
        return new Vector2(x, y);
    }

    void SetUpFieldPlayerMapSize()
    {
        fieldPlayer.fieldMapWidth = fieldData.mapWidth; // フィールドの幅を設定
        fieldPlayer.fieldMapHeight = fieldData.mapHeight; // フィールドの高さを設定
    }

    GameObject CreateTile(string name, Sprite sprite, Vector2 position, string sortingLayer, string layerName)
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

    GameObject InstantiateBuildingPrefab(GameObject prefab, Vector2 position, string sortingLayer, string layerName, BuildingType type)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        Building building = obj.GetComponent<Building>();
        building.Setup(type); // 建物のアイコンを設定

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = sortingLayer;
        obj.layer = LayerMask.NameToLayer(layerName);
        obj.AddComponent<BoxCollider2D>();
        return obj;
    }

    // マップを初期化
    void ClearMap()
    {
        hitTargetPin.RemoveTargetPin(); // ターゲットピンを削除
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
        return new Vector2(x * tileSize + (tileSize / 2), (fieldData.mapHeight - y) * tileSize - (tileSize / 2)); // マップの中心を考慮して座標を計算
    }

    Vector2 GetCharacterPositionFromCoordinate(int x, int y)
    {
        return new Vector2(x * tileSize + tileSize, (fieldData.mapHeight - y) * tileSize); // マップの中心を考慮して座標を計算
    }

    private void ResetFieldInfoPanel()
    {
        currentBuildingBase = null;
        fieldInfoPanel.gameObject.SetActive(false);
        // fieldInfoPanel.Setup(fieldData.mapBase);
    }

    public Battler GetEnemy()
    {
        return fieldData.GetRundamEnemy();
    }

    public void FieldInfoPanleSwitch(bool isOpen)
    {
        fieldInfoPanel.gameObject.SetActive(isOpen);
    }
}