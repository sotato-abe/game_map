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

    public float scale = 3.0f;


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

    public IEnumerator OnPointer(bool focusFlg)
    {
        float time = 0.05f;
        float currentTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        if (focusFlg)
        {
            targetScale = new Vector3(scale, scale, scale);
        }
        while (currentTime < time)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
