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
    float targetX;
    public UnityAction OnExecute;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform をキャッシュ
        targetX = rectTransform.anchoredPosition.x + targetOffsetX; // 相対移動
    }

    public void SetTurnBattlerIcon(Sprite battlerImage)
    {
        gameObject.SetActive(true); // オブジェクトをアクティブにする
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>(); // 確実に取得する

        iconImage.sprite = battlerImage;
        StartCoroutine(WaitAndMoveToLeft(0.1f)); // **0.1秒待ってから移動開始**
    }

    public void SetActive(bool isActiveFlg)
    {
        if (isActive == isActiveFlg) return;  // 状態が変わらない場合は処理をスキップ

        isActive = isActiveFlg;

        if (isActive)
        {
            StartCoroutine(MoveToLeft());
        }
        else
        {
            StopAllCoroutines(); // コルーチンを停止
        }
    }
    private IEnumerator WaitAndMoveToLeft(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!gameObject.activeInHierarchy) yield break; // **オブジェクトが非アクティブなら中断**

        StartCoroutine(MoveToLeft());
    }

    private IEnumerator MoveToLeft()
    {
        while (isActive && rectTransform.anchoredPosition.x > targetX)
        {
            rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (isActive)
        {
            ExecuteTurn();
        }
    }

    private void ExecuteTurn()
    {
        OnExecute?.Invoke();
    }
}
