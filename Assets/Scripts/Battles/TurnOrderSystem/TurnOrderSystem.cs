using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattler turnBattlerPrefab;
    [SerializeField] GameObject battlerList;
    private TurnBattler targetTurnBattler;
    private List<TurnBattler> turnBattlerList = new List<TurnBattler>();
    private List<Battler> battlers = new List<Battler>(); // 保存用
    private bool isActive = false;

    public void SetUpBattlerTurns(List<Battler> newBattlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in battlerList.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();

        // バトラー情報を保存
        battlers = new List<Battler>(newBattlers);
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
        Debug.Log($"GenerateTurnBattler:{isActive}");
    }

    public void SetActive(bool isActiveFlg)
    {
        Debug.Log($"TurnOrderSystem:SetActive:{isActiveFlg}");
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合は処理をスキップ

        isActive = isActiveFlg;

        if (turnBattlerList.Count == 0)
        {
            Debug.Log("No TurnBattlerList");
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
        Debug.Log($"{turnBattler.battler.Base.name} のターン開始");
        targetTurnBattler = turnBattler;
        SetActive(false);
        StartCoroutine(TestTurn());
    }

    private IEnumerator TestTurn()
    {
        yield return new WaitForSeconds(3.0f);
        EndTurn();
    }

    public void EndTurn()
    {
        if (targetTurnBattler)
        {
            targetTurnBattler.EndTurn();
            Debug.Log($"{targetTurnBattler.battler.Base.name} のターン終了");
            // targetTurnBattler = null;
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
        turnBattlerList.Clear();
    }
}
