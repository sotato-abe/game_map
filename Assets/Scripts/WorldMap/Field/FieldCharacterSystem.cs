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
    List<FieldEnemy> fieldCharacters = new List<FieldEnemy>(); // フィールドの敵リスト
    [SerializeField] GameObject fieldCharacterFront; // フィールドキャンバス
    [SerializeField] GameObject fieldCharacterBehind; // フィールドキャンバス

    public IEnumerator appearanceEnemy(List<Battler> battlers)
    {
        // count分の敵をフィールドに出現させる
        foreach (Battler battler in battlers)
        {
            // ランダムな位置を取得
            (Vector3 targetPos, bool isRight, bool isFront) = GetRundomArroundFloorPosition();
            FieldEnemy enemy = null;
            GameObject targetPosition = isFront ? fieldCharacterFront : fieldCharacterBehind;
            int reversal = isRight ? -1 : 1; // 向きの設定
            enemy = Instantiate(Oldman_fieldEnemy, targetPos, Quaternion.identity, targetPosition.transform);
            enemy.SetUp(battler); // バトラーの設定を行う
            enemy.transform.localScale = new Vector3(reversal, 1, 1); // 左向きにする
            fieldPlayer.transform.localScale = new Vector3(reversal * -1, 1, 1); // 左向きにする
            fieldCharacters.Add(enemy); // 生成した敵をリストに追加
            yield return new WaitForSeconds(0.3f);
        }
        yield break; // 全ての敵を出現させたらnullを返す
    }

    public void RemoveAllCharacter()
    {
        // 全てのフィールドキャラクターを削除
        foreach (FieldEnemy enemy in fieldCharacters)
        {
            Destroy(enemy.gameObject); // ゲームオブジェクトを削除
        }
        fieldCharacters.Clear(); // リストをクリア
    }

    public void RemoveFieldCharacter(Battler battler)
    {
        // 指定されたバトラーに対応する敵を削除
        FieldEnemy enemyToRemove = fieldCharacters.Find(enemy => enemy.Battler == battler);
        if (enemyToRemove != null)
        {
            fieldCharacters.Remove(enemyToRemove); // リストから削除
            Destroy(enemyToRemove.gameObject); // ゲームオブジェクトを削除
        }
    }

    private (Vector3, bool, bool) GetRundomArroundFloorPosition(int range = 1)
    {
        // フィールドのランダムな位置を取得
        Vector3 pos = fieldPlayer.transform.position;
        // 0 は除外、-range ~ rangeの範囲でランダムな座標を取得
        int x = 0;
        int y = 0;
        bool isFront = true;
        bool isRight = true;

        // (0,0) 以外になるまでランダムに取得
        while (x == 0 && y == 0)
        {
            x = Random.Range(-range, range + 1); // 上限は含まれないので +1
            y = Random.Range(-range, range + 1);
        }
        if (y < 0)
            isFront = false;
        if (x < 0)
            isRight = false;

        Vector3 targetPos = new Vector3(pos.x + x, pos.y + y, 0); // プレイヤーの位置にランダムなオフセットを加算

        return (targetPos, isRight, isFront);
    }
}