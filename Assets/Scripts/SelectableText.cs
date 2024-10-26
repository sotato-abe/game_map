using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectableText : MonoBehaviour
{
    TextMeshProUGUI text;

    float textAlpha = 1f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSelectedColor(bool selected)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        if (selected)
        {
            text.color = Color.yellow;
            Color textColor = text.color;
            textColor.a = Mathf.Clamp(textAlpha, 0f, 1f);
            text.color = textColor;
        }
        else
        {
            text.color = Color.white;
            Color textColor = text.color;
            textColor.a = Mathf.Clamp(textAlpha, 0f, 1f);
            text.color = textColor;
        }
    }

    public void SetTextValidity(float alpha)
    {
        textAlpha = alpha;

        // Color textColor = text.color;
        // textColor.a = Mathf.Clamp(alpha, 0f, 1f); // 透明度を 0～1 に制限
        // text.color = textColor;
        // Debug.Log($"SetTextValidity:{alpha}:{text.color.a}:{text.text}");
    }
}
