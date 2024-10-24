using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDialog : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image dialogBackground;
    [SerializeField] float letterPerSecond;

    public IEnumerator TypeDialog(string line)
    {
        StartCoroutine(SetTransparency(1f));
        text.SetText("");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(SetTransparency(0.5f));
    }

    public IEnumerator SetTransparency(float alpha)
    {
        // TextMeshProUGUI の透明度を変更
        if (text != null)
        {
            Color textColor = text.color;
            textColor.a = Mathf.Clamp(alpha, 0f, 1f); // 透明度を 0～1 に制限
            text.color = textColor;
        }

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
