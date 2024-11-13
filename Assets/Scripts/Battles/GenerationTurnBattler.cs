using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTurnBattler : MonoBehaviour
{
    public Battler Battler { get; set; }
    [SerializeField] TurnBattler turnBattlerPrefab;
    private TurnOrderSystem turnOrderSystem;
    private bool isActive = true;
    private float generationInterval;
    private Coroutine generationCoroutine;
    private float startTime; // 生成を開始した時点の時間
    private float elapsedTime = 0f; // 中断した時点の経過時間

    // 初期化時にBattlerとTurnOrderSystemをセット
    public void Initialize(Battler battler, TurnOrderSystem system)
    {
        Battler = battler;
        turnOrderSystem = system;
        SetGenerationInterval();
        generationCoroutine = StartCoroutine(GenerationUnit());
    }

    // 速度に応じた生成間隔を計算
    private void SetGenerationInterval()
    {
        generationInterval = Mathf.Max(1f, 30f / Battler.Speed);
    }

    // Activeフラグを受け取って、生成を再開または一時停止
    public void SetActive(bool isActiveFlag)
    {
        if (isActive == isActiveFlag) return;

        isActive = isActiveFlag;
        SetTurnBattlerActive();

        if (isActive)
        {
            // 再開時に中断していた経過時間を引き継ぎコルーチンを再開
            if (generationCoroutine == null)
            {
                generationCoroutine = StartCoroutine(GenerationUnit());
            }
        }
        else
        {
            // 一時停止時にコルーチンを停止し、現在の経過時間を保持
            if (generationCoroutine != null)
            {
                elapsedTime =  Time.time - startTime;
                StopCoroutine(generationCoroutine);
                generationCoroutine = null;
            }
        }
    }

    // TurnBattlerを生成するコルーチン
    private IEnumerator GenerationUnit()
    {
        while (true)
        {
            if (isActive)
            {
                // 残りの生成間隔を計算
                startTime = Time.time;
                float remainingTime = generationInterval - elapsedTime;
                if (remainingTime > 0)
                {
                    yield return new WaitForSeconds(remainingTime);
                }

                // TurnBattlerを生成
                TurnBattler newTurnBattler = Instantiate(turnBattlerPrefab, transform);
                newTurnBattler.Initialize(Battler, turnOrderSystem);
                newTurnBattler.gameObject.SetActive(true);

                // 生成が完了したら経過時間をリセット
                elapsedTime = 0f;
            }
            else
            {
                // 非Activeの間は経過時間を維持したまま待機
                yield return null;
            }
        }
    }

    // TurnBattlerのアクティブ状態を更新
    public void SetTurnBattlerActive()
    {
        Debug.Log($"[{Battler.Base.Name}] SetTurnBattlerActive:{isActive}");
        foreach (Transform child in transform)
        {
            TurnBattler turnBattlerUnit = child.GetComponent<TurnBattler>();
            if (turnBattlerUnit != null)
            {
                turnBattlerUnit.SetActive(isActive);
            }
        }
    }
}
