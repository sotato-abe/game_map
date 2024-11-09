using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBattlerUnit : MonoBehaviour
{
    public Battler Battler { get; set; }
    public bool IsActive { get; private set; } = false;

    [SerializeField] Image image;
    private TurnOrderSystem turnOrderSystem;
    private float moveSpeed = 100f;
    private float targetPositionX = 25f;
    private bool hasTurnExecuted = false; // ターンが完了したかどうかを示すフラグ

    public void Initialize(Battler battler, TurnOrderSystem system)
    {
        if (battler.Base != null)
        {
            Battler = battler;
            turnOrderSystem = system;
            image.sprite = battler.Base.Sprite;
        }
        else
        {
            Debug.LogError("Battler.Base is null. Cannot assign sprite.");
        }
    }

    public void StartMoving()
    {
        if (!IsActive)
        {
            IsActive = true;
            StartCoroutine(MoveToLeft());
        }
    }

    public void StopMoving()
    {
        if (IsActive)
        {
            IsActive = false;
        }
    }

    private IEnumerator MoveToLeft()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        while (rectTransform.anchoredPosition.x > targetPositionX)
        {
            if (IsActive)
            {
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }

        // ターン完了時の処理が複数回実行されないようにする
        if (turnOrderSystem != null && !hasTurnExecuted)
        {
            hasTurnExecuted = true; // ターン完了フラグを立てる
            turnOrderSystem.ExecuteTurn(this);
        }

        Destroy(gameObject);
    }
}
