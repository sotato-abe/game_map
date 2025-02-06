using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnBattlerIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;  // 表示用のTextMeshProUGUIフィールド
    private bool isActive = true;
    private float moveSpeed = 300f;
    private float targetPositionX = -630f;
    public delegate void ExecuteTurnDelegate(TurnBattlerIcon turnBattlerIcon);
    public event ExecuteTurnDelegate ExecuteTurn;

    public void SetCharacter(Sprite characterSprite)
    {
        iconImage.sprite = characterSprite;
        StartCoroutine(MoveToLeft());
    }

    public void SetActive(bool activeFlg)
    {
        isActive = activeFlg;
    }

    private IEnumerator MoveToLeft()
    {

        RectTransform rectTransform = GetComponent<RectTransform>();
        while (rectTransform.anchoredPosition.x > targetPositionX)
        {
            if (isActive)
            {
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }

        // ターン開始を通知し、ターン終了後にこのオブジェクトを破棄
        ExecuteTurn?.Invoke(this);
        // ターン終了したTurnBattlerIconを削除
        Destroy(this.gameObject);
    }
}

