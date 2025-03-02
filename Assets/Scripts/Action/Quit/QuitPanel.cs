using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class QuitPanel : Panel
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Quit();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnActionExit?.Invoke();
        }
    }

    private void Quit()
    {
        isActive = false;
        OnActionExecute?.Invoke();
    }
}
