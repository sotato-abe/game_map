using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] StatusIconList statusIconList;

    public void SetStatusIcon(Status status)
    {
        text.text = status.val.ToString();
        image.sprite = statusIconList.GetIcon(status.type);
    }
}

