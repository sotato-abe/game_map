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
        text.alignment = TextAlignmentOptions.Center;
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
            text.fontSize = 27f;
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
    }

    public void SetText(string line)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.text = line;
    }
}
