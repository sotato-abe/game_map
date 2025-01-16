using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldInfoPanel : MonoBehaviour
{
    [SerializeField] Title title;
    [SerializeField] Description description;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetInfo(MapBase mapBase)
    {
        StartCoroutine(title.TypeTitle(mapBase.Name));
        StartCoroutine(description.TypeDescription(mapBase.Description));
    }
}
