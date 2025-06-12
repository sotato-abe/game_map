using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnBarText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnBarText;
    private Coroutine flashCoroutine;


    public void SetText(string text)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        turnBarText.text = text;
        turnBarText.color = new Color(turnBarText.color.r, turnBarText.color.g, turnBarText.color.b, 1f);
        flashCoroutine = StartCoroutine(FlashAndHide());
    }

    private IEnumerator FlashAndHide()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        float blinkSpeed = 0.3f;

        while (elapsed < duration)
        {
            // Alpha切り替えで点滅
            float alpha = Mathf.PingPong(Time.time * (1f / blinkSpeed), 1f);
            var color = turnBarText.color;
            color.a = alpha;
            turnBarText.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 最後に非表示
        turnBarText.text = "";
    }
}
