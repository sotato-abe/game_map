using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattler turnBattlerPrefab;
    [SerializeField] GameObject battlerList;
    [SerializeField] GameObject turnBar;
    [SerializeField] BattleSystem battleSystem;
    private TurnBattler targetTurnBattler;
    private List<TurnBattler> turnBattlerList = new List<TurnBattler>();
    private List<Battler> battlers = new List<Battler>(); // 保存用
    private bool isActive = false;

    public void SetupBattlerTurns(List<Battler> newBattlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in battlerList.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();

        // バトラー情報を保存
        battlers = new List<Battler>(newBattlers);
        turnBar.gameObject.SetActive(true);
        // 生成を開始
        GenerateTurnBattler();
    }

    private void GenerateTurnBattler()
    {
        foreach (Battler battler in battlers)
        {
            TurnBattler turnBattler = Instantiate(turnBattlerPrefab, battlerList.transform);
            turnBattler.OnExecuteTurn += ExecuteTurn;
            turnBattler.SetBattler(battler);
            turnBattlerList.Add(turnBattler);
        }
        SetActive(true);
    }

    public void SetActive(bool isActiveFlg)
    {
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合は処理をスキップ

        isActive = isActiveFlg;

        if (turnBattlerList.Count == 0)
        {
            Debug.LogWarning("No TurnBattlerList");
        }

        foreach (TurnBattler turnBattler in turnBattlerList)
        {
            if (turnBattler != null)
            {
                turnBattler.SetActive(isActive);  // TurnBattlerのSetActiveを呼び出し
            }
        }
    }

    public void ExecuteTurn(TurnBattler turnBattler)
    {
        StartCoroutine(ExecuteTurnCoroutine(turnBattler));
    }

    private IEnumerator ExecuteTurnCoroutine(TurnBattler turnBattler)
    {
        targetTurnBattler = turnBattler;
        SetActive(false);

        if (targetTurnBattler.battler.Base.Name == "Sola") // TODO: 仮の分岐ちゃんとプレイヤーと他を分ける
        {
            Debug.Log($"プレイヤーのターン開始");
            battleSystem.StartActionSelection();
        }
        else
        {
            Debug.Log($"敵のターン開始");
            StartCoroutine(battleSystem.EnemyAttack());
        }
        yield return null;
    }

    public void EndTurn()
    {
        if (targetTurnBattler)
        {
            targetTurnBattler.EndTurn();
        }

        SetActive(true);
    }

    public void BattlerEnd()
    {
        foreach (TurnBattler turnBattler in turnBattlerList)
        {
            turnBattler.OnExecuteTurn -= ExecuteTurn;
            turnBattler.EndBattle();
            Destroy(turnBattler.gameObject);
        }
        turnBar.gameObject.SetActive(false);
        turnBattlerList.Clear();
    }
}
