using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] EnegyIconList enegyIconList;

    public void SetCostIcon(Enegy cost)
    {
        text.text = cost.val.ToString();
        SetCostIcon(cost.type);
    }

    public void SetCostIcon(EnegyType type)
    {
        image.sprite = enegyIconList.GetIcon(type);
    }
}

