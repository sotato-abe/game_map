using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldInfoPanel : MonoBehaviour
{
    [SerializeField] Title title;
    [SerializeField] Description description;

    public void Setup(MapBase mapBase)
    {
        transform.gameObject.SetActive(true);
        StartCoroutine(title.TypeTitle(mapBase.Name));
        StartCoroutine(description.TypeDescription(mapBase.Description));
    }
}
