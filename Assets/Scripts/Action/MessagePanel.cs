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

    public IEnumerator TypeDialog(string line)
    {
        text.SetText("");
        Debug.Log($"Line:{line}");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        yield return new WaitForSeconds(2f);
    }

    // 現在使用していない
    public IEnumerator SetTransparency(float alpha)
    {
        // 背景のImageコンポーネントの透明度を変更
        if (dialogBackground != null)
        {
            Color bgColor = dialogBackground.color;
            bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
            dialogBackground.color = bgColor;
        }
        yield return null;
    }
}
