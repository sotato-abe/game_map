using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image image;
    [SerializeField] float padding = 30f;

    private RectTransform backImageRectTransform;

    private void Start()
    {
        backImageRectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator TypeTitle(string line)
    {
        title.SetText("");
        foreach (char letter in line)
        {
            title.text += letter;
            yield return new WaitForSeconds(0f);
        }
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
