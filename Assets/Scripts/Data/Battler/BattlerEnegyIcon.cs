using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerEnegyIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] EnegyIconList enegyIconList;

    public void SetEnegy(Enegy cost)
    {
        text.text = cost.val.ToString();
        SetEnegyIcon(cost.type);
    }

    public void SetEnegyIcon(EnegyType type)
    {
        image.sprite = enegyIconList.GetIcon(type);
    }
}

