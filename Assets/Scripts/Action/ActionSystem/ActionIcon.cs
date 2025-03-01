using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionIcon : MonoBehaviour
{
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI text;
    private bool isActive = false;

    private float defaultWidth = 70f;
    private float defaultHeight = 80f;
    private float defaultFontSize = 12f; // デフォルトのフォントサイズ
    private float activeScale = 1.5f;
    private float scaleDuration = 0.05f; // スケール変更の時間

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetAction(ActionType actionType)
    {
        text.SetText("");
        string actionText = System.Enum.GetName(typeof(ActionType), actionType);
        text.text = actionText;
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;

        // コルーチンを開始してスムーズにサイズとフォントサイズを変更
        StopAllCoroutines();

        float targetWidth = isActive ? defaultWidth * activeScale : defaultWidth;
        float targetHeight = isActive ? defaultHeight * activeScale : defaultHeight;
        float targetFontSize = isActive ? defaultFontSize * activeScale : defaultFontSize;

        StartCoroutine(ResizeOverTime(targetWidth, targetHeight, targetFontSize));
    }

    private IEnumerator ResizeOverTime(float targetWidth, float targetHeight, float targetFontSize)
    {
        float elapsedTime = 0f;
        Vector2 startSize = rectTransform.sizeDelta;
        Vector2 endSize = new Vector2(targetWidth, targetHeight);

        float startFontSize = text.fontSize;
        float endFontSize = targetFontSize;

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / scaleDuration;

            rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            text.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);

            yield return null;
        }

        rectTransform.sizeDelta = endSize;
        text.fontSize = endFontSize;
    }
}
