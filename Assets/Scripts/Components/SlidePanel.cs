using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlidePanel : MonoBehaviour
{
    public RectTransform rectTransform;
    public Vector3 activePosition = new Vector3(0, 0, 0);
    public Vector3 inactivePosition = new Vector3(0, 0, 0);
    public bool isActive = false;

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
        if (isActive)
        {
            StartCoroutine(Slide(activePosition));
        }
        else
        {
            StartCoroutine(Slide(inactivePosition));
        }
    }

    private IEnumerator Slide(Vector3 targetPosition)
    {
        Vector3 startPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

}