using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPoint : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] RectTransform backRectTransform;
    private float padding = 40f;

    public void SetPoint(int skillPoint)
    {
        title.SetText(skillPoint.ToString());
        ResizePlate();
    }

    private void ResizePlate()
    {
        if (title == null || backRectTransform == null)
        {
            Debug.LogError("title または backRectTransform が null");
            return;
        }

        float newWidth = title.preferredWidth + padding;
        backRectTransform.sizeDelta = new Vector2(newWidth, backRectTransform.sizeDelta.y);
    }
}
