using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EnegyCounter : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image updownIcon;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite ButtryIcon;
    [SerializeField] Sprite upIcon;
    [SerializeField] Sprite downIcon;
    [SerializeField] private TextMeshProUGUI val;
    [SerializeField] private TextMeshProUGUI cal;
    public EnegyType enegyType;
    Color32 upColor = new Color32(2, 171, 229, 255);
    Color32 downColor = new Color32(245, 52, 124, 255);

    public delegate void EnegyUpDelegate(EnegyType enegyType);
    public event EnegyUpDelegate EnegyUp;

    public void SetEnegyCounter(EnegyCount enegy)
    {
        val.text = enegy.max.ToString();
        cal.text = enegy.cal.ToString();
        enegyType = enegy.type;
        icon.sprite = enegy.type == EnegyType.Life ? lifeIcon : ButtryIcon;
        SetUpDownIcon(enegy.max, enegy.cal);
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

    public void OnEnegyUp()
    {
        EnegyUp?.Invoke(enegyType);
    }
}

