using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattler turnBattlerPrefab;
    [SerializeField] GameObject turnLane;
    private TurnBattler targetTurnBattler;
    private List<TurnBattler> turnBattlerList = new List<TurnBattler>();
    private List<Battler> battlers = new List<Battler>(); // 保存用
    private Coroutine generateCoroutine;
    private bool isGenerating = false;
    private bool isActive = false;

    public void SetUpBattlerTurns(List<Battler> newBattlers)
    {
        // 既存の子オブジェクトをすべて削除
        foreach (Transform child in turnLane.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();

        // バトラー情報を保存
        battlers = new List<Battler>(newBattlers);
        // 生成を開始
        GenerateTurnBattler();
    }

    private IEnumerator GenerateTurnBattler()
    {
        foreach (Battler battler in battlers)
        {
            if (!isGenerating) yield break;

            TurnBattler turnBattler = Instantiate(turnBattlerPrefab, turnLane.transform);
            turnBattler.OnExecuteTurn += ExecuteTurn;
            turnBattler.SetBattler(battler);
            turnBattlerList.Add(turnBattler);
            Debug.Log($"battlerName: {battler.Base.Name}");

            // Speed に応じた間隔で次のアイコンを生成
            float interval = Mathf.Clamp(10.0f / battler.Speed, 1.0f, 10.0f);
            yield return new WaitForSeconds(interval);
        }
        isGenerating = false; // 生成完了後フラグをリセット
    }

    public void SetActive(bool isActiveFlg)
    {
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合は処理をスキップ

        Debug.Log($"TurnOrderSystem:SetActive:{isActive}");

        if(turnBattlerList.Count == 0){
            Debug.Log("No TurnBattlerList");
        }

        foreach (TurnBattler turnBattler in turnBattlerList)
        {
            Debug.Log($"TurnOrderSystem:turnBattler{turnBattler.battler.Base.Name}");
            if (turnBattler != null)
            {
                turnBattler.SetActive(isActive);  // TurnBattlerのSetActiveを呼び出し
            }
        }
    }

    public void ExecuteTurn(Battler battler)
    {
        Debug.Log($"{battler.Base.name} のターン開始");
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
            targetTurnBattler.OnExecuteTurn -= ExecuteTurn;
            Destroy(targetTurnBattler.gameObject);
            targetTurnBattler = null;
        }

        SetActive(true);
    }

    public void BattlerEnd()
    {
        foreach (Transform child in turnLane.transform)
        {
            Destroy(child.gameObject);
        }
        turnBattlerList.Clear();
    }
}
