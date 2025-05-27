using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariableDialog : Dialog
{
    [SerializeField] RectTransform backRectTransform;
    protected virtual float PaddingHeight => 110f;
    protected virtual float dialogWidth => 300f;

    public void ResizeDialog()
    {
        description.ForceMeshUpdate();
        float newHeight = description.preferredHeight + PaddingHeight;
        backRectTransform.sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
