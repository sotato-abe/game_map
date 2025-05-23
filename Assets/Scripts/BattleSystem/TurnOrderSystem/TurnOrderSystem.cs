using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattler turnBattlerPrefab;
    [SerializeField] GameObject battlerList;
    [SerializeField] GameObject turnBar;
    [SerializeField] GameObject turnLane;
    [SerializeField] BattleSystem battleSystem;
    private TurnBattler targetTurnBattler;
    private List<TurnBattler> turnBattlerList = new List<TurnBattler>();
    private List<Battler> battlers = new List<Battler>(); // 保存用
    private bool isActive = false;

    private Battler playerBattler;

    public void TurnOrderClear()
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in battlerList.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();
        turnBar.gameObject.SetActive(true);
        isActive = false;
    }

    public void SetupPlayerBattler(Battler player)
    {
        playerBattler = player;
        SetTurnBattler(playerBattler);
    }

    public void SetTurnBattler(Battler battler)
    {
        TurnBattler turnBattler = Instantiate(turnBattlerPrefab, battlerList.transform);
        turnBattler.OnExecuteTurn += ExecuteTurn;
        turnBattler.SetBattler(battler);
        turnBattler.SetLane(turnLane);
        turnBattlerList.Add(turnBattler);
    }

    public void SetActive(bool isActiveFlg)
    {
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合は処理をスキップ

        isActive = isActiveFlg;
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

        if (targetTurnBattler.battler == playerBattler) // TODO: 仮の分岐ちゃんとプレイヤーと他を分ける
        {
            battleSystem.StartActionSelection();
        }
        else
        {
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
        isActive = false;
        turnBar.gameObject.SetActive(false);
        turnBattlerList.Clear();
    }

    public void RemoveTurnBattler(Battler targetBattler)
    {
        foreach (TurnBattler turnBattler in turnBattlerList)
        {
            if (turnBattler.battler == targetBattler)
            {

                turnBattler.OnExecuteTurn -= ExecuteTurn;
                turnBattler.EndBattle();
                Destroy(turnBattler.gameObject);
                turnBattlerList.Remove(turnBattler);
                break;
            }
        }
    }
}
