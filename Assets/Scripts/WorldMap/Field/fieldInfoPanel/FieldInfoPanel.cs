using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldInfoPanel : MonoBehaviour
{
    [SerializeField] Title title;
    [SerializeField] Description description;
    [SerializeField] FieldInfoIcon icon;
    private List<string> descriptionList = new List<string>();

    public void Setup(FieldBase fieldBase)
    {
        transform.gameObject.SetActive(true);
        string titleText = "Unknown Field";
        string descriptionText = "Seaching...";
        if (fieldBase != null)
        {
            titleText = fieldBase.Name;
            descriptionText = fieldBase.Description;
        }
        StartCoroutine(title.TypeTitle(titleText));
        StartCoroutine(description.TypeDescription(descriptionText));
    }

    public void SetupBuilding(BuildingBase buildingBase)
    {
        transform.gameObject.SetActive(true);
        icon.SetIconMotion(MotionType.Jump);
        StartCoroutine(title.TypeTitle(buildingBase.Name));
        StartCoroutine(description.TypeDescription(buildingBase.Description));
    }
}
