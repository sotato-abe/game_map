using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPointPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] RectTransform backImageRectTransform;
    private float padding = 40f;

    public void SetPoint(int skillPoint)
    {
        title.SetText(skillPoint.ToString());
        ResizePlate();
    }

    private void ResizePlate()
    {
        if (title == null || backImageRectTransform == null)
        {
            Debug.LogError("title または backImageRectTransform が null");
            return;
        }

        float newWidth = title.preferredWidth + padding;
        backImageRectTransform.sizeDelta = new Vector2(newWidth, backImageRectTransform.sizeDelta.y);
    }
}
