using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class QuitPanel : Panel
{
    public UnityAction OnActionExecute;
    public UnityAction OnActionExit;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Escape();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    private void Escape()
    {
        isActive = false;
        OnActionExecute?.Invoke();
    }
}
