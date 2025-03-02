using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DeckPanel : Panel
{
    public UnityAction OnActionExecute;
    public UnityAction OnActionExit;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isActive = true;
        }

        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnActionExit?.Invoke();
            }
        }
    }
}
