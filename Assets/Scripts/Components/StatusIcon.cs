using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] StatusIconList statusIconList;
    public StatusType statusType;

    public delegate void StatusUpDelegate(StatusType statusType);
    public event StatusUpDelegate StatusUp;

    public void SetStatusIcon(Status status)
    {
        text.text = status.val.ToString();
        statusType = status.type;
        image.sprite = statusIconList.GetIcon(status.type);
    }

    public void OnStatusUp()
    {
        StatusUp?.Invoke(statusType);
    }
}

