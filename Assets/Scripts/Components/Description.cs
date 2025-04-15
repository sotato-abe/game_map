using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Description : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image image;

    [SerializeField] float padding = 90f;
    [SerializeField] RectTransform backImageRectTransform;

    public IEnumerator TypeDescription(string line)
    {
        description.SetText("");
        foreach (char letter in line)
        {
            description.text += letter;
            ResizePlate();
            yield return new WaitForSeconds(0f);
        }
    }

    private void ResizePlate()
    {
        if (description == null || backImageRectTransform == null)
        {
            Debug.LogError("description または backImageRectTransform が null");
            return;
        }

        float newHeight = description.preferredHeight + padding;
        backImageRectTransform.sizeDelta = new Vector2(backImageRectTransform.sizeDelta.x, newHeight);
    }
}
