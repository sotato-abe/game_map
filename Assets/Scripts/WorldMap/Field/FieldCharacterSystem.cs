using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//　役割：フィールドの生成を行う
//　マップ生成：座標を受け取ると、WorldMapSystemからフィールドデータを取得
//　フィールドデータからタイルセットを選択しフィールドの描画を行う

public class FieldCharacterSystem : MonoBehaviour
{
    [SerializeField] FieldPlayer fieldPlayer; //キャラクター
    [SerializeField] FieldEnemy Slyme_fieldEnemy; //キャラクター
    [SerializeField] FieldEnemy Oldman_fieldEnemy; //キャラクター
    List<FieldEnemy> fieldEnemies = new List<FieldEnemy>(); // フィールドの敵リスト
    [SerializeField] GameObject fieldCanvas; // フィールドキャンバス

    public void appearanceEnemy()
    {
        (Vector3 targetPos, bool isRight) = GetRundomArroundFloorPosition();
        FieldEnemy enemy = Instantiate(Oldman_fieldEnemy, targetPos, Quaternion.identity, fieldCanvas.transform);
        if (isRight)
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1); // 左向きにする
            fieldPlayer.transform.localScale = new Vector3(1, 1, 1); // 左向きにする
        }
        else
        {
            enemy.transform.localScale = new Vector3(1, 1, 1); // 右向きにする
            fieldPlayer.transform.localScale = new Vector3(-1, 1, 1); // 左向きにする
        }
        fieldEnemies.Add(enemy); // 生成した敵をリストに追加
    }

    public void RemoveEnemy()
    {
        // 敵を削除
        foreach (FieldEnemy enemy in fieldEnemies)
        {
            Destroy(enemy.gameObject); // 敵を削除
        }
        fieldEnemies.Clear(); // リストをクリア
    }

    private (Vector3, bool) GetRundomArroundFloorPosition(int range = 1)
    {
        // フィールドのランダムな位置を取得
        Vector3 pos = fieldPlayer.transform.position;
        // 0 は除外、-range ~ rangeの範囲でランダムな座標を取得
        int x = 0;
        int y = 0;
        bool isRight = true;

        // (0,0) 以外になるまでランダムに取得
        while (x == 0 && y == 0)
        {
            x = Random.Range(-range, range + 1); // 上限は含まれないので +1
            y = Random.Range(-range, range + 1);
        }
        if (x < 0)
        {
            isRight = false;
        }

        Vector3 targetPos = new Vector3(pos.x + x, pos.y + y, 0); // プレイヤーの位置にランダムなオフセットを加算

        return (targetPos, isRight);
    }
}