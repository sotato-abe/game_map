using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDialog : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI characterName;
    [SerializeField] public TextMeshProUGUI description;
    [SerializeField] RectTransform backRectTransform;

    private float PaddingHeight = 90f;
    // private float PaddingWidth = 90f;
    private float dialogWidth = 400f;

    public void Setup(Battler battler)
    {
        characterName.SetText(battler.Base.Name);
        description.text = battler.Base.Description;
        ResizeDialog();
    }

    public void ResizeDialog()
    {
        description.ForceMeshUpdate();
        float newHeight = description.preferredHeight + PaddingHeight;
        backRectTransform.sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
