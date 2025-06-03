using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StatusCounter : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image updownIcon;
    [SerializeField] Sprite upIcon;
    [SerializeField] Sprite downIcon;
    [SerializeField] private TextMeshProUGUI val;
    [SerializeField] private TextMeshProUGUI cal;
    public StatusType statusType;
    Color32 upColor = new Color32(3, 137, 229, 255);
    Color32 downColor = new Color32(245, 52, 124, 255);

    public delegate void StatusUpDelegate(StatusType statusType);
    public event StatusUpDelegate StatusUp;

    public void SetStatusCounter(StatusCount status)
    {
        val.text = status.val.ToString();
        cal.text = status.cal.ToString();
        statusType = status.type;
        StatusData data = StatusDatabase.Instance?.GetData(status.type);
        icon.sprite = data.icon;
        SetUpDownIcon(status.val, status.cal);
    }

    private void SetUpDownIcon(int val, int cal)
    {
        if (val == cal)
        {
            updownIcon.color = new Color(0, 0, 0, 0); // 非表示
        }
        else if (val < cal)
        {
            updownIcon.sprite = upIcon;
            updownIcon.color = upColor;
        }
        else
        {
            updownIcon.sprite = downIcon;
            updownIcon.color = downColor;
        }
    }

    public void OnStatusUp()
    {
        StatusUp?.Invoke(statusType);
    }
}

