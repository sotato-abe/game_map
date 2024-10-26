using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectableText : MonoBehaviour
{
    TextMeshProUGUI text;

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
        }
        else
        {
            text.color = Color.white;
        }
    }

    public void SetTextValidity(bool validityFlg)
    {
        Debug.Log("SetTextValidity");
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        Color textColor = text.color;
        textColor.a = validityFlg ? 1f : 0.5f; // `selectable` に基づき透明度を設定
        text.color = textColor;
    }
}
