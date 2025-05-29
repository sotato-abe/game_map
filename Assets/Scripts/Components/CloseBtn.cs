using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CloseBtn : Unit
{
    void Start()
    {
        scale = 1.4f;
    }
    public void OnPointerEnter()
    {
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        StartCoroutine(OnPointer(false));
    }
}
