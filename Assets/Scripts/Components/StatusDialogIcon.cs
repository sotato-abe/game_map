using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StatusDialogIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI text;
    public StatusType statusType;
    Color32 upColor = new Color32(2, 171, 229, 255);
    Color32 downColor = new Color32(245, 52, 124, 255);

    public void SetStatusIcon(Status status)
    {
        text.text = status.val.ToString();
        statusType = status.type;
        StatusData data = StatusDatabase.Instance?.GetData(status.type);
        icon.sprite = data.icon;
        SetUpDownColor(0 <= status.val);
    }

    private void SetUpDownColor(bool isPlus)
    {
        if (isPlus)
        {
            icon.color = upColor;
            text.color = upColor;
        }
        else
        {
            icon.color = downColor;
            text.color = downColor;
        }
    }
}

