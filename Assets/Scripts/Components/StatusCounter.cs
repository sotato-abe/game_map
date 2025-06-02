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
    [SerializeField] private TextMeshProUGUI val;
    [SerializeField] private TextMeshProUGUI cal;
    public StatusType statusType;

    public delegate void StatusUpDelegate(StatusType statusType);
    public event StatusUpDelegate StatusUp;

    public void SetStatusCounter(StatusCount status)
    {
        val.text = status.val.ToString();
        cal.text = status.cal.ToString();
        statusType = status.type;
        StatusData data = StatusDatabase.Instance?.GetData(status.type);
        icon.sprite = data.icon;
    }

    public void OnStatusUp()
    {
        StatusUp?.Invoke(statusType);
    }
}

