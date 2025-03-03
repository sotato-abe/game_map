using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Escape();
        }
    }

    private void Escape()
    {
        isActive = false;
        OnActionExecute?.Invoke();
    }
}
