using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NamePlate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image backPanel;
    [SerializeField] float padding = 20f;

    public int minWidth = 10; // 最小幅を設定

    public void SetName(string name)
    {
        nameText.SetText(name);
        ResizePlate();
    }

    // テキストの長さによってbackPanelのサイズを変更する
    private void ResizePlate()
    {
        // backPanelのサイズを変更
        float newWidth = nameText.preferredWidth + padding;
        newWidth = Mathf.Max(newWidth, minWidth);
        backPanel.rectTransform.sizeDelta = new Vector2(newWidth, backPanel.rectTransform.sizeDelta.y);
    }

}
