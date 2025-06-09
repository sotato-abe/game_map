using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite unknounSprite;
    [SerializeField] RectTransform backRectTransform;

    int defaultHeight = 80;
    int defaultwidth = 360;
    float time = 0.1f;

    public void Setup(Sprite sprite, string name, string description)
    {
        transform.gameObject.SetActive(true);
        image.sprite = sprite;
    }

    public void SetUnknown()
    {
        image.sprite = unknounSprite;
        transform.gameObject.SetActive(false);
    }

    public void OnPointerEnter()
    {
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        StartCoroutine(OnPointer(false));
    }

    public IEnumerator OnPointer(bool isActive)
    {
        float currentTime = 0f;

        if (isActive)
        {
            Vector2 originalSize = backRectTransform.sizeDelta;
            Vector2 targetSize = new Vector2(defaultwidth, defaultwidth);
            while (currentTime < time)
            {
                backRectTransform.sizeDelta = Vector2.Lerp(originalSize, targetSize, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            backRectTransform.sizeDelta = targetSize;
        }
        else
        {
            Vector2 originalSize = backRectTransform.sizeDelta;
            Vector2 targetSize = new Vector2(defaultwidth, defaultHeight);
            while (currentTime < time)
            {
                backRectTransform.sizeDelta = Vector2.Lerp(originalSize, targetSize, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            backRectTransform.sizeDelta = targetSize;
        }
    }
}
