using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//　actionPanelを切り替えるボタンのアイコン
public class ActionIcon : MonoBehaviour, IPointerEnterHandler
{
    public UnityAction<ActionType> OnPointerEnterAction;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] ActionIconList actionIconList;


    public ActionType type;
    [SerializeField] private bool isActive = false;
    private float defaultWidth = 50f;
    private float defaultHeight = 50f;
    private float defaultFontSize = 12f; // デフォルトのフォントサイズ
    private float activeScale = 1.5f;
    private float scaleDuration = 0.05f; // スケール変更の時間

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

        public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke(type);
    }

    public void SetAction(ActionType actionType)
    {
        type = actionType;
        image.sprite = actionIconList.GetIcon(type);
        string actionText = System.Enum.GetName(typeof(ActionType), type);
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
