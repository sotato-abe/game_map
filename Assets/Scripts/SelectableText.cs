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
}
