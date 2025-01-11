using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float letterPerSecond;
    [SerializeField] Image dialogBackground;
    private Coroutine fadeCoroutine;

    public IEnumerator TypeDialog(string line)
    {
        SetPanelValidity(0.9f);
        text.SetText("");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        // StartCoroutine(FadeOutAlpha());
    }

    public void SetPanelValidity(float targetAlpha, float duration = 0.5f)
    {
        // 既存のフェードコルーチンが動いている場合は停止
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // 新しいフェードコルーチンを開始
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha, duration));
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = dialogBackground.color.a; // 現在のアルファ値
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration); // 線形補間でアルファ値を更新

            Color bgColor = dialogBackground.color;
            bgColor.a = alpha;
            dialogBackground.color = bgColor;

            yield return null; // 次のフレームまで待機
        }

        // 最後に目標のアルファ値に確定
        Color finalColor = dialogBackground.color;
        finalColor.a = targetAlpha;
        dialogBackground.color = finalColor;

        // コルーチンの参照をクリア
        fadeCoroutine = null;
    }

    private IEnumerator FadeOutAlpha()
    {
        yield return new WaitForSeconds(5f);
        SetPanelValidity(0.2f);
    }
}
