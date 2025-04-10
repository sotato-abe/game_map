using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PercentageBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] Image backImage;

    float animationSpeed = 5f;
    Coroutine animCoroutine;

    public void SetBar(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);

        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }

        animCoroutine = StartCoroutine(CountBar(percentage));
    }

    private IEnumerator CountBar(float target)
    {
        while (Mathf.Abs(bar.fillAmount - target) > 0.001f)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, target, Time.deltaTime * animationSpeed);
            yield return null;
        }

        bar.fillAmount = target;
    }

    public IEnumerator FullBar()
    {
        float target = 1f;
        while (Mathf.Abs(bar.fillAmount - target) > 0.001f)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, target, Time.deltaTime * animationSpeed);
            yield return null;
        }

        bar.fillAmount = target;
    }

    public void ResetBar()
    {
        bar.fillAmount = 0f;
    }
}
