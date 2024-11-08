using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattlerUnit turnBattlerUnitPrefab;     // キャラクターのプレハブ
    [SerializeField] GameObject turnBattlerList;
    [SerializeField] BattleSystem battleSystem;
    int intervalBetweenBattlers = 10; // キャラクター生成間隔
    float moveSpeed = 100f; // キャラクターの移動速度

    private bool isActive = true; // 動きが止まっているかどうかを示すフラグ

    void Init()
    {
    }

    public void SetUpBattlerTurns(List<Battler> battlers)
    {
        // 戦闘の速さに基づいて並び順を決定
        battlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));  // 高速な順に並べる

        foreach (Transform child in turnBattlerList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Battler battler in battlers)
        {
            // Speed に応じた間隔を設定
            int interval = (int)(100 / battler.Speed) * intervalBetweenBattlers; // Speedに応じた間隔
            int currentPosition = 50;

            for (int i = interval; i < 700; i += interval)
            {
                // TurnBattlerUnit を生成し、GameObject を取得
                GameObject battlerObject = Instantiate(turnBattlerUnitPrefab, turnBattlerList.transform).gameObject;
                battlerObject.SetActive(true);

                // TurnBattlerUnit の初期化
                TurnBattlerUnit turnBattlerUnit = battlerObject.GetComponent<TurnBattlerUnit>();
                turnBattlerUnit.Initialize(battler);

                // RectTransform の設定
                RectTransform rectTransform = battlerObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 0.5f);
                rectTransform.anchorMax = new Vector2(0f, 0.5f);
                rectTransform.pivot = new Vector2(0f, 0.5f);

                // anchoredPosition を使って左側の中央からの相対位置に配置
                rectTransform.anchoredPosition = new Vector2(currentPosition + i, 0f);

                // キャラクターを左に移動させるコルーチンを開始
                StartCoroutine(MoveBattlerToLeft(rectTransform));
            }
        }
        isActive = true;
    }

    private IEnumerator MoveBattlerToLeft(RectTransform rectTransform)
    {
        // 左端の位置（ターン実行位置）を設定
        float targetPositionX = 25f;

        while (rectTransform.anchoredPosition.x > targetPositionX && isActive)
        {
            // 少しずつ左に移動
            rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (isActive)
        {
            isActive = false;
            ExecuteTurn(rectTransform);
        }
    }

    private void ExecuteTurn(RectTransform rectTransform)
    {
        StartCoroutine(battleSystem.SetBattleState(BattleState.ActionSelection));
        // rectTransform.gameObject.SetActive(false);
        Destroy(rectTransform.gameObject);
        Debug.Log($"ターンを実行: {rectTransform.gameObject.name}");
        // ターン実行時の処理をここに記述
    }

    public void SetActive(bool flg)
    {
        isActive = flg;
        Debug.Log(isActive);
        if (isActive)
        {
            // すべてのバトラーオブジェクトを再度移動させる
            foreach (Transform child in turnBattlerList.transform)
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                StartCoroutine(MoveBattlerToLeft(rectTransform));
            }
        }
    }
}
