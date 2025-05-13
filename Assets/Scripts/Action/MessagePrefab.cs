using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] RectTransform backImageRectTransform;
    [SerializeField] TextMeshProUGUI text;

    private int lineWidth = 700;
    private float padding = 0f;

    public void SetMessage(Message message)
    {
        text.SetText(message.messageText);
        ResizePlate();
        image.sprite = message.sprite;
    }

    private void ResizePlate()
    {
        if (text == null || backImageRectTransform == null)
        {
            Debug.LogError("description または backImageRectTransform が null");
            return;
        }

        float newHeight = text.preferredHeight + padding;
        backImageRectTransform.sizeDelta = new Vector2(lineWidth, newHeight);
    }
}
