using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattler : MonoBehaviour
{
    private TurnBattlerIcon turnBattlerIcon;
    public Battler battler;
    private List<TurnBattlerIcon> turnBattlerIconList = new List<TurnBattlerIcon>();

    private bool isActive = false;

    public delegate void ExecuteTurnDelegate(Battler battler);
    public event ExecuteTurnDelegate OnExecuteTurn;

    public void SetBattler(Battler targetBattler)
    {
        battler = targetBattler;
        GenerateTurnBattlerIcon();
    }

    public void SetActive(bool activeFlg)
    {
        Debug.Log($"TurnBattler:SetActive:{activeFlg}");
        if (isActive == activeFlg) return;  // 状態が変わらない場合、処理をスキップ

        isActive = activeFlg;

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
            // TurnBattlerIconの生成と親の設定
            turnBattlerIcon = TurnBattlerIconPool.Instance.GetIcon();
            turnBattlerIcon.transform.SetParent(transform, false); // 親オブジェクトの設定は1回だけ
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
