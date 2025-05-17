using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariableDialog : Dialog
{
    [SerializeField] RectTransform backImageRectTransform;
    protected virtual float PaddingHeight => 110f;
    private float dialogWidth = 300f;

    public void ResizeDialog()
    {
        description.ForceMeshUpdate();
        float newHeight = description.preferredHeight + PaddingHeight;
        backImageRectTransform.sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
