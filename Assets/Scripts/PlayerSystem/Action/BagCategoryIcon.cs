using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagCategoryIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] Image backImage;
    private bool isActive = false;

    public void SetCategory(BagCategory category)
    {
        text.SetText("");
        string categoryText = System.Enum.GetName(typeof(BagCategory), category);
        text.text = categoryText;
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;

        // 背景の透明度を変更する。
        Color bgColor = GetComponent<Image>().color;
        bgColor.a = isActive ? 1f : 0f;
        GetComponent<Image>().color = bgColor;
    }
}
