using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 【機能】
// TurnBattlerの元となるプレハブ
// Battlerのターンイメージを描画する
// 非アクティブ時に移動を停止する
// アクティブ時に一定速度で左に進む（速度はビートに依存する）
// 左端にたどり着くとターン開始をTurnOrderSystemにBattler情報を通知する

public class TurnBattler : MonoBehaviour
{
    [SerializeField] Image image;
    public Battler Battler { get; set; }
    public bool IsActive { get; private set; } = true;
    private TurnOrderSystem turnOrderSystem;
    private float moveSpeed = 300f;
    private float targetPositionX = -600f;

    public void Initialize(Battler battler, TurnOrderSystem system)
    {
        if (battler.Base != null)
        {
            Battler = battler;
            image.sprite = battler.Base.Sprite;
            turnOrderSystem = system;
            StartCoroutine(MoveToLeft());
        }
        else
        {
            Debug.LogError("Battler.Base is null. Cannot assign sprite.");
        }
    }

    public void SetActive(bool activeFlg)
    {
        Debug.Log($"{Battler.Base.Name}:SetActive:{activeFlg}");
        IsActive = activeFlg;
    }

    private IEnumerator MoveToLeft()
    {
        if (turnOrderSystem == null)
        {
            Debug.LogError("TurnOrderSystem is not assigned.");
            yield break;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        while (rectTransform.anchoredPosition.x > targetPositionX)
        {
            if (IsActive)
            {
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            }
            else
            {
                Debug.Log("MoveToLeft:Stop");
            }
            yield return null;
        }

        // ターン開始を通知し、ターン終了後にこのオブジェクトを破棄
        yield return StartCoroutine(turnOrderSystem.ExecuteTurn(this));
        Destroy(gameObject);
    }
}
