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

    [SerializeField] Color recoveryColor = new Color(2, 115, 229, 255);
    [SerializeField] Color damageColor = new Color(243, 52, 74, 255);

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
            image.color = damageColor;
            text.color = damageColor;
        }
        else
        {
            image.color = recoveryColor;
            text.color = recoveryColor;
        }
    }

    public void OnEnegyUp()
    {
        EnegyUp?.Invoke(enegyType);
    }
}