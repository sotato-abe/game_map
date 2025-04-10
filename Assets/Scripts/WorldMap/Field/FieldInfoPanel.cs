using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldInfoPanel : MonoBehaviour
{
    [SerializeField] Title title;
    [SerializeField] Description description;
    private List<string> descriptionList = new List<string>();

    public void Setup(MapBase mapBase)
    {
        transform.gameObject.SetActive(true);
        string titleText = "Unknown Field";
        string descriptionText = "Seaching...";
        if (mapBase != null)
        {
            titleText = mapBase.Name;
            descriptionText = mapBase.Description;
        }
        StartCoroutine(title.TypeTitle(titleText));
        StartCoroutine(description.TypeDescription(descriptionText));
    }

    public void SetupBuilding(BuildingBase buildingBase)
    {
        transform.gameObject.SetActive(true);
        StartCoroutine(title.TypeTitle(buildingBase.Name));
        StartCoroutine(description.TypeDescription(buildingBase.Description));
    }
}
