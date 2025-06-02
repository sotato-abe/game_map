using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EnegyCounter : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Sprite ButtryIcon;
    [SerializeField] Image updownIcon;
    [SerializeField] private TextMeshProUGUI val;
    [SerializeField] private TextMeshProUGUI cal;
    public EnegyType enegyType;

    public delegate void EnegyUpDelegate(EnegyType enegyType);
    public event EnegyUpDelegate EnegyUp;

    public void SetEnegyCounter(EnegyCount enegy)
    {
        val.text = enegy.val.ToString();
        cal.text = enegy.cal.ToString();
        enegyType = enegy.type;
        icon.sprite = enegy.type == EnegyType.Life ? lifeIcon : ButtryIcon;
    }

    public void OnEnegyUp()
    {
        EnegyUp?.Invoke(enegyType);
    }
}

