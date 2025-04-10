using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnegyIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] EnegyIconList enegyIconList;

    public EnegyType enegyType;

    public delegate void EnegyUpDelegate(EnegyType EnegyType);
    public event EnegyUpDelegate EnegyUp;

    public void SetCostIcon(Enegy cost)
    {
        text.text = cost.val.ToString();
        enegyType = cost.type;
        image.sprite = enegyIconList.GetIcon(cost.type);
    }

    public void OnEnegyUp()
    {
        EnegyUp?.Invoke(enegyType);
    }
}

