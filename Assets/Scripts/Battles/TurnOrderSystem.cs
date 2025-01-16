using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機能
// バトルが開始されるとシステムが開始される
// バトルに参加しているキャラクター（Battler）のGenerationTurnBattlerを作成する。
// ターンバーのアクティブを制御する。
// TurnBattlerのターン実行を受け取りbattleSystemにターン実行を通知する。
// キャラクターのスピードが変更されたときにBattlerのスピードに応じてGenerationTurnBattlerとTurnBattlerを生成し直す。

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] GenerationTurnBattler generationTurnBattlerPrefab;
    [SerializeField] BattleSystem battleSystem;

    private TurnBattler targetBattler;

    private List<GenerationTurnBattler> generationTurnBattlers = new List<GenerationTurnBattler>();

    public void SetUpBattlerTurns(List<Battler> battlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        generationTurnBattlers.Clear();

        // スピード順にソート
        battlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        // 各バトラーに対してGenerationTurnBattlerを生成し設定
        foreach (Battler battler in battlers)
        {
            GenerationTurnBattler generationTurnBattler = Instantiate(generationTurnBattlerPrefab, transform);
            generationTurnBattler.Initialize(battler, this);
            generationTurnBattlers.Add(generationTurnBattler);
        }
    }

    public void SetActive(bool isActiveFlg)
    {
        // GenerationTurnBattlerとTurnBattlerのアクティブ状態を制御
        foreach (GenerationTurnBattler generationTurnBattler in generationTurnBattlers)
        {
            generationTurnBattler.SetActive(isActiveFlg);
        }
    }

    public IEnumerator ExecuteTurn(TurnBattler turnBattler)
    {
        Debug.Log($"ターンを実行: {turnBattler.Battler.Base.Name}");
        targetBattler = turnBattler;
        SetActive(false); // ターン実行中は他のバトラーを非アクティブ化

        if (turnBattler.Battler.Base.Name == "Sola") // TODO: 仮の分岐
        {
            yield return StartCoroutine(battleSystem.SetBattleState(BattleState.ActionSelection));
        }
        else
        {
            yield return StartCoroutine(battleSystem.EnemyAttack());
        }
        Debug.Log("ExecuteTurnEnd");
        // SetActive(true); // TODO ターン終了後に再びアクティブ化
        // Destroy(turnBattler.gameObject); // ターン終了したTurnBattlerを削除
    }

    public void EndTurn()
    {
        if (targetBattler)
        {
            Destroy(targetBattler.gameObject); // ターン終了したTurnBattlerを削除
            targetBattler = null;
        }
        SetActive(true);
    }
}
