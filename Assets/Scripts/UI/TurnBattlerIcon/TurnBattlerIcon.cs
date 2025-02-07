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
    private bool isMoving = false; // 停止/再開の制御用

    public UnityAction OnExecute;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform をキャッシュ
    }

    public void SetTurnBattlerIcon(Sprite battlerImage)
    {
        iconImage.sprite = battlerImage;
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveToLeft());
        }
    }

    public void SetActive(bool activeFlg)
    {
        isActive = activeFlg;
    }

    private IEnumerator MoveToLeft()
    {
        float targetX = rectTransform.anchoredPosition.x + targetOffsetX; // 相対移動
        while (isMoving && rectTransform.anchoredPosition.x > targetX)
        {
            if (isActive)
            {
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }

        if (isMoving) // 停止が指示されていない場合のみ実行
        {
            Debug.Log("ターン開始: " + gameObject.name);
            OnExecute?.Invoke();
        }
    }

    public void StopMoving()
    {
        isMoving = false; // 強制停止
    }
}
