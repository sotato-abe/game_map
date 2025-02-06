using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTurnBattler : MonoBehaviour
{
    public Battler Battler { get; set; }
    [SerializeField] TurnBattlerIcon turnBattlerIcon;
    private TurnOrderSystem turnOrderSystem;
    private bool isActive = true;
    private float generationInterval;
    private Coroutine generationCoroutine;
    private float generatStartTime; // 生成を開始した時点の時間
    private float interruptionStartTime; // ターンなどで生成を中断開始した時点の時間
    private float interruptionTime = 0f; // 中断にかけた経過時間

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
        generationInterval = Mathf.Max(1f, 10f / Battler.Speed);
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
                if (interruptionStartTime > 0)
                {
                    interruptionTime += Time.time - interruptionStartTime; // 中断開始時点からの経過時間を計算
                    interruptionStartTime = 0f;
                }
                generationCoroutine = StartCoroutine(GenerationUnit());
            }
        }
        else
        {
            // 一時停止時にコルーチンを停止し、現在の経過時間を保持
            if (generationCoroutine != null)
            {
                interruptionStartTime = Time.time; // 中断開始時点の時間を記録
                StopCoroutine(generationCoroutine);
                generationCoroutine = null;
            }
        }
    }

    // TurnBattlerIconを生成するコルーチン
    private IEnumerator GenerationUnit()
    {
        while (true)
        {
            if (isActive)
            {
                // 残りの生成間隔を計算
                if (generatStartTime == 0)
                {
                    generatStartTime = Time.time;
                }

                float generatTime = (Time.time - generatStartTime) - interruptionTime;
                float remainingTime = Mathf.Max(0, generationInterval - generatTime);
                if (remainingTime > 0)
                {
                    yield return new WaitForSeconds(remainingTime);
                }

                // TurnBattlerIconを生成
                TurnBattlerIcon newTurnBattler = Instantiate(turnBattlerIcon, transform);
                newTurnBattler.SetCharacter(Battler.Base.Sprite);
                newTurnBattler.gameObject.SetActive(true);

                // 生成が完了したら経過時間をリセット
                generatStartTime = 0f;
                interruptionTime = 0f;
            }
            else
            {
                // 非Activeの間は経過時間を維持したまま待機
                yield return null;
            }
        }
    }

    // TurnBattlerIconのアクティブ状態を更新
    public void SetTurnBattlerActive()
    {
        foreach (Transform child in transform)
        {
            TurnBattlerIcon turnBattlerUnit = child.GetComponent<TurnBattlerIcon>();
            if (turnBattlerUnit != null)
            {
                turnBattlerUnit.SetActive(isActive);
            }
        }
    }
}
