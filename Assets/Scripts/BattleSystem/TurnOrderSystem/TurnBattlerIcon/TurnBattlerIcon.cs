using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattlerIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image frameImage;
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float targetOffsetX = -700f; // 相対的な移動距離
    [SerializeField] Color activeColor = new Color(133, 10, 240, 255);
    [SerializeField] Color runningColor = new Color(133, 10, 240, 255);
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
        frameImage.color = runningColor; // 背景を透明にする
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
            frameImage.color = activeColor;
            OnExecute?.Invoke();
            StartCoroutine(JumpMotion());
        }
    }

    private IEnumerator JumpMotion()
    {
        float bounceHeight = 40f;
        float damping = 0.5f;
        float gravity = 5000f;
        float groundY = transform.position.y;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping;  // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);  // 最後に位置を調整
    }
}
