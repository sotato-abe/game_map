using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機能
// バトルが開始されるとシステムが開始される
// バトルに参加しているキャラクター（Battler）のturnBattlerを作成する。
// ターンバーのアクティブを制御する。
// キャラクターのスピードが変更されたときにBattlerのスピードに応じてTurnBattlerIconを生成し直す。

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattlerIcon turnBattlerPrefab;
    [SerializeField] GameObject turnLane;
    private TurnBattlerIcon targetBattlerIcon;
    private List<TurnBattlerIcon> turnBattlerList = new List<TurnBattlerIcon>();

    public void SetUpBattlerTurns(List<Battler> battlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in turnLane.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();

        // スピード順にソート
        battlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        // 各バトラー毎にurnBattlerIconを生成し設定
        foreach (Battler battler in battlers)
        {
            TurnBattlerIcon turnBattlerIcon = Instantiate(turnBattlerPrefab, turnLane.transform);
            turnBattlerIcon.ExecuteTurn += ExecuteTurn;
            turnBattlerIcon.SetCharacter(battler.Base.Sprite);
            turnBattlerList.Add(turnBattlerIcon);
            Debug.Log($"battlerName{battler.Base.Name}");
        }
    }

    public void SetActive(bool isActiveFlg)
    {
        Debug.Log("TurnOrderSystem:SetActive");
        foreach (TurnBattlerIcon turnBattler in turnBattlerList)
        {
            turnBattler.SetActive(isActiveFlg);
        }
    }

    public void ExecuteTurn(TurnBattlerIcon turnBattler)
    {
        Debug.Log($"{turnBattler.name} のターン開始");
        targetBattlerIcon = turnBattler;
        // BattleSystemにBattlerを受け渡しターンを開始
        EndTurn();
    }

    public void EndTurn()
    {
        if (targetBattlerIcon)
        {
            targetBattlerIcon.ExecuteTurn -= ExecuteTurn;
            Destroy(targetBattlerIcon.gameObject);
            targetBattlerIcon = null;
        }
    }

    public void BattlerEnd()
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in turnLane.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();
    }
}
