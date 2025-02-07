using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattler : MonoBehaviour
{
    [SerializeField] TurnBattlerIcon turnBattlerIcon;
    public Battler battler;
    private List<TurnBattlerIcon> turnBattlerIconList = new List<TurnBattlerIcon>();

    private bool isActive = false;

    public delegate void ExecuteTurnDelegate(Battler battler);
    public event ExecuteTurnDelegate OnExecuteTurn;

    public void SetBattler(Battler targetBattler)
    {
        Debug.Log($"TurnBattler:SetBattler:{targetBattler.Base.Name}");
        battler = targetBattler;
        GenerateTurnBattlerIcon();
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;  // 状態が変わらない場合、処理をスキップ

        isActive = activeFlg;

        Debug.Log($"TurnBattler:SetActive:{turnBattlerIconList.Count}");

        // 一度だけ状態変更を行う
        foreach (TurnBattlerIcon icon in turnBattlerIconList)
        {
            icon.SetActive(isActive);
        }
    }

    private void GenerateTurnBattlerIcon()
    {
        if (turnBattlerIcon == null)
        {
            turnBattlerIcon = TurnBattlerIconPool.Instance.GetIcon();

            if (turnBattlerIcon == null) // 取得に失敗した場合の対策
            {
                Debug.LogError("TurnBattlerIconがnullです。アイコンの取得に失敗しました。");
                return;
            }

            turnBattlerIcon.transform.SetParent(transform, false);
            turnBattlerIcon.gameObject.SetActive(true); // オブジェクトをアクティブにする
        }
        turnBattlerIcon.SetTurnBattlerIcon(battler.Base.Sprite);
        turnBattlerIconList.Add(turnBattlerIcon);
    }


    public void RemoveTurnBattler()
    {
        if (turnBattlerIcon != null)
        {
            // TurnBattlerIconをリリースしてリストから削除
            TurnBattlerIconPool.Instance.ReleaseIcon(turnBattlerIcon);
            turnBattlerIconList.Remove(turnBattlerIcon);
            turnBattlerIcon = null; // 参照をクリア
        }
    }

    public void ExecuteTurnBattler()
    {
        OnExecuteTurn?.Invoke(battler);
    }
}
