using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionIcon : MonoBehaviour
{
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool isActive = true;
    [SerializeField] private float activeScale = 1.2f;
    [SerializeField] private float inactiveScale = 1.0f;
    [SerializeField] private float scaleDuration = 0.2f; // スケール変更の時間

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetActive(!isActive);
        }
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
        Debug.Log($"ActionIcon is now {isActive}");

        // コルーチンを開始してスムーズにスケール変更
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(isActive ? activeScale : inactiveScale));
    }

    private IEnumerator ScaleOverTime(float targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startScale = rectTransform.localScale;
        Vector3 endScale = new Vector3(targetScale, targetScale, 1f);

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / scaleDuration);
            yield return null;
        }

        rectTransform.localScale = endScale;
    }
}
