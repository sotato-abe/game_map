using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattler : MonoBehaviour
{
    [SerializeField] TurnBattlerIcon turnBattlerIconPrefab;
    [SerializeField] GameObject turnLane;
    public Battler battler;
    private List<TurnBattlerIcon> turnBattlerIconList = new List<TurnBattlerIcon>();

    private bool isActive = false;
    float elapsedTime = 0f;
    float interval; // 最小0.5秒の待機時間を確保

    private Coroutine generateIconCoroutine;

    public delegate void ExecuteTurnDelegate(TurnBattler battler);
    public event ExecuteTurnDelegate OnExecuteTurn;

    public void SetBattler(Battler targetBattler)
    {
        Debug.Log($"TurnBattler:SetBattler:{targetBattler.Base.Name}");
        battler = targetBattler;
        interval = Mathf.Max(0.5f, 10f / battler.Base.Speed); // 最小0.5秒の待機時間を確保

        // 既にコルーチンが動作している場合は停止
        if (generateIconCoroutine != null)
        {
            StopCoroutine(generateIconCoroutine);
        }

        // アイコン生成のコルーチンを開始
        generateIconCoroutine = StartCoroutine(GenerateTurnBattlerIconsRepeatedly());
    }

    public void SetActive(bool isActiveFlg)
    {
        Debug.Log($"TurnBattler:SetActive:{battler.Base.Name}:{isActiveFlg}");
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合、処理をスキップ

        isActive = isActiveFlg;
        // TurnBattlerIconsそれぞれに状態変更を行う
        foreach (TurnBattlerIcon icon in turnBattlerIconList)
        {
            icon.SetActive(isActive);
        }

        // 状態が無効になった場合、アイコン生成を停止
        if (!isActive && generateIconCoroutine != null)
        {
            StopCoroutine(generateIconCoroutine);
            generateIconCoroutine = null;
        }
        else if (isActive && generateIconCoroutine == null)
        {
            generateIconCoroutine = StartCoroutine(GenerateTurnBattlerIconsRepeatedly());
        }
    }
    private IEnumerator GenerateTurnBattlerIconsRepeatedly()
    {
        while (true) // 常にループするが、isActive が false の時は一時停止する
        {
            // isActive が true になるまで待機
            yield return new WaitUntil(() => isActive);

            while (elapsedTime < interval)
            {
                if (!isActive) yield break; // isActive が false になったら即時停止
                elapsedTime += Time.deltaTime;
                yield return null; // 毎フレーム待機
            }

            // 生成前に再度チェック
            if (isActive)
            {
                GenerateTurnBattlerIcon();
            }
            elapsedTime = 0f;
        }
    }


    private void GenerateTurnBattlerIcon()
    {
        Debug.Log($"TurnBattler:GenerateTurnBattlerIcon:{isActive}");
        TurnBattlerIcon turnBattlerIcon = Instantiate(turnBattlerIconPrefab, turnLane.transform);
        turnBattlerIcon.SetTurnBattlerIcon(battler.Base.Sprite);
        turnBattlerIcon.OnExecute += ExecuteTurnBattler;
        turnBattlerIconList.Add(turnBattlerIcon);
    }

    public void ExecuteTurnBattler()
    {
        OnExecuteTurn?.Invoke(this);
    }

    public void EndTurn()
    {
        // もしリストが空でない場合、先頭のアイコンを削除
        if (turnBattlerIconList.Count > 0)
        {
            TurnBattlerIcon firstIcon = turnBattlerIconList[0]; // リストの先頭要素を取得

            // イベントからハンドラーを解除
            firstIcon.OnExecute -= ExecuteTurnBattler;
            turnBattlerIconList.RemoveAt(0); // リストからその要素を削除
            Destroy(firstIcon.gameObject); // ゲームオブジェクトを破壊
        }
    }

    public void EndBattle()
    {
        foreach (TurnBattlerIcon icon in turnBattlerIconList)
        {
            Destroy(icon.gameObject);
        }

        // コルーチンを停止
        if (generateIconCoroutine != null)
        {
            StopCoroutine(generateIconCoroutine);
            generateIconCoroutine = null;
        }
    }
}
