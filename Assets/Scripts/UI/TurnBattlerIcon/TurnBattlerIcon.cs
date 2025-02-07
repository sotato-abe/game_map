using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattlerIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float targetOffsetX = -700f; // 相対的な移動距離
    private RectTransform rectTransform;
    private bool isActive = true;
    public UnityAction OnExecute;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform をキャッシュ
    }

    public void SetTurnBattlerIcon(Sprite battlerImage)
    {
        gameObject.SetActive(true); // アクティブ化
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>(); // 確実に取得する

        iconImage.sprite = battlerImage;
        SetActive(true);
    }

    public void SetActive(bool activeFlg)
    {
        Debug.Log($"TurnBattlerIcon:SetActive:{activeFlg}");
        isActive = activeFlg;

        if (isActive)
        {
            gameObject.SetActive(true); // オブジェクトをアクティブにする
            StartCoroutine(MoveToLeft());
        }
        else
        {
            StopAllCoroutines(); // コルーチンを停止する
            gameObject.SetActive(false); // 非アクティブ化
        }
    }


    private IEnumerator MoveToLeft()
    {
        float targetX = rectTransform.anchoredPosition.x + targetOffsetX; // 相対移動
        while (isActive && rectTransform.anchoredPosition.x > targetX)
        {
            rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (isActive)
        {
            Debug.Log("ターン開始: " + gameObject.name);
            OnExecute?.Invoke();
        }
    }
}
