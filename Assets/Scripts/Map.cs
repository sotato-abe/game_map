
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab1, groundPrefab2, wallPrefab1, wallPrefab2, sectionLinePrefab, buildingPrefab1, entryPrefab; //各種プレファブ
    [SerializeField] MapBase mapBase; //各種プレファブ
    RougeGenerator mapGenerator; //RougeGenerator型の変数
    int[,] map; //マップデータ
    float tileSize; //プレファブのサイズ
    Vector2 mapCenterPos; //マップのセンター位置

    void Start()
    {
        mapGenerator = new RougeGenerator(mapBase); //RougeGeneratorインスタンス生成
        map = mapGenerator.GenerateMap();//マップ作成
        PlaceTiles();//プレファブを並べる処理
    }

    void PlaceTiles()
    {
        tileSize = groundPrefab1.GetComponent<SpriteRenderer>().bounds.size.x; //タイルサイズ取得
        mapCenterPos = new Vector2(mapBase.MapWidth * tileSize / 2, mapBase.MapHeight * tileSize / 2); //中心座標取得
        for (int y = 0; y < mapBase.MapHeight; y++) //マップデータ読込ループ
        {
            for (int x = 0; x < mapBase.MapWidth; x++)
            {
                int tileType = map[x, y]; //マップの種類取得
                Vector2 pos = GetWorldPositionFromTile(x, y);//座標を計算

                //マップの種類毎にゲームオブジェクト作成
                if (tileType == 0) // Layer
                {
                    // Instantiate(groundPrefab1, pos, Quaternion.Euler(0, 0, 0f));
                }
                else if (tileType == 1) // AreaLine
                {
                    // Instantiate(groundPrefab1, pos, Quaternion.Euler(0, 0, 0f));
                }
                else if (tileType == 2) // Ground
                {
                    Instantiate(groundPrefab1, pos, Quaternion.Euler(0, 0, 0f));
                }
                else if (tileType == 3) // Wall
                {
                    // Groundが上下左右に存在するかどうかを判定する
                    if (Random.Range(0, 100) < 80)
                    {
                        Instantiate(wallPrefab1, pos, Quaternion.Euler(0, 0, 0f));
                    }
                    else
                    {
                        Instantiate(wallPrefab2, pos, Quaternion.Euler(0, 0, 0f));
                    }
                }
                else if (tileType == 4) // Object
                {
                    Instantiate(groundPrefab2, pos, Quaternion.Euler(0, 0, 0f));
                }
                else if (tileType == 5) // Building
                {
                    Instantiate(buildingPrefab1, pos, Quaternion.Euler(0, 0, 0f));
                }
                else if (tileType == 6) // Building
                {
                    Instantiate(entryPrefab, pos, Quaternion.Euler(0, 0, 0f));
                }
            }
        }
    }
    //座標を取得するメソッド
    Vector2 GetWorldPositionFromTile(int x, int y)
    {
        return new Vector2(x * tileSize, (mapBase.MapHeight - y) * tileSize) - mapCenterPos;
    }
}