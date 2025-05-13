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
    [SerializeField] Image backImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] ActionIconList actionIconList;


    public ActionType type;
    [SerializeField] private bool isActive = false;
    private float defaultWidth = 50f;
    private float defaultHeight = 50f;
    private float defaultFontSize = 12f; // デフォルトのフォントサイズ
    private float activeScale = 2.0f;
    private float scaleDuration = 0.05f; // スケール変更の時間

    private RectTransform rectTransform;

    [SerializeField] Color activeColor = new Color(133, 10, 240, 255);
    [SerializeField] Color stopColor = new Color(0, 0, 0, 200);

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
        if (isActive)
        {
            SetColor(activeColor);
        }
        else
        {
            SetColor(stopColor);
        }

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
        var layout = GetComponent<LayoutElement>();

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / scaleDuration;

            Vector2 currentSize = Vector2.Lerp(startSize, endSize, t);
            rectTransform.sizeDelta = currentSize;
            text.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);
            layout.preferredHeight = currentSize.y;

            yield return null;
        }

        rectTransform.sizeDelta = endSize;
        text.fontSize = endFontSize;
        layout.preferredHeight = targetHeight;
    }

    private void SetColor(Color color)
    {
        backImage.color = color;
    }
}
