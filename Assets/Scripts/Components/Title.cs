using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image image;
    private RectTransform backImageRectTransform;

    private void Start()
    {
    }

    public IEnumerator TypeTitle(string line)
    {
        title.SetText("");
        foreach (char letter in line)
        {
            title.text += letter;
            yield return new WaitForSeconds(0f);
        }
    }
}
