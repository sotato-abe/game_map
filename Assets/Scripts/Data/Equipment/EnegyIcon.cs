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

    Color32 upColor = new Color32(2, 171, 229, 255);
    Color32 downColor = new Color32(245, 52, 124, 255);

    public EnegyType enegyType;

    public delegate void EnegyUpDelegate(EnegyType EnegyType);
    public event EnegyUpDelegate EnegyUp;

    public void SetCostIcon(Enegy cost)
    {
        text.text = cost.val.ToString();
        enegyType = cost.type;
        image.sprite = enegyIconList.GetIcon(cost.type);
    }

    public void SetColor(bool isDamage)
    {
        if (isDamage)
        {
            image.color = downColor;
            text.color = downColor;
        }
        else
        {
            image.color = upColor;
            text.color = upColor;
        }
    }

    public void OnEnegyUp()
    {
        EnegyUp?.Invoke(enegyType);
    }
}