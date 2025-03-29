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

    public void SetCostIcon(Enegy cost)
    {
        text.text = cost.val.ToString();
        image.sprite = enegyIconList.GetIcon(cost.type);
    }
}

