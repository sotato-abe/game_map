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
    private TurnBattlerIcon targetBattlerIcon;
    private List<TurnBattlerIcon> turnBattlerList = new List<TurnBattlerIcon>();

    private void Start()
    {
        turnBattlerPrefab.ExecuteTurn += ExecuteTurn;
    }

    public void SetUpBattlerTurns(List<Battler> battlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();

        // スピード順にソート
        battlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        // 各バトラー毎にurnBattlerIconを生成し設定
        foreach (Battler battler in battlers)
        {
            TurnBattlerIcon turnBattlerIcon = Instantiate(turnBattlerPrefab, transform);
            turnBattlerIcon.SetCharacter(battler.Base.Sprite);
            turnBattlerList.Add(turnBattlerIcon);
            Debug.Log($"battlerName{battler.Base.Name}");
        }
    }

    public void SetActive(bool isActiveFlg)
    {
        Debug.Log("TurnOrderSystem:SetActive");
        // TurnBattlerIconのアクティブ状態を制御
        foreach (TurnBattlerIcon turnBattler in turnBattlerList)
        {
            turnBattler.SetActive(isActiveFlg);
        }
    }

    public void ExecuteTurn(TurnBattlerIcon turnBattler)
    {
        Debug.Log($"{turnBattler.name} のターン開始");
        targetBattlerIcon = turnBattler;
        EndTurn();
    }

    public void EndTurn()
    {
        if (targetBattlerIcon)
        {
            Destroy(targetBattlerIcon.gameObject); // ターン終了したTurnBattlerIconを削除
            targetBattlerIcon = null;
        }
    }
}
