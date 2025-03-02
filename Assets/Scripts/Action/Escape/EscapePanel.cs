using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EscapePanel : Panel
{
    [SerializeField] private TextMeshProUGUI text;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Escape();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = false;
            OnActionExit?.Invoke();
        }
    }

    public void SetEscapePanel()
    {

    }

    private void Escape()
    {
        isActive = false;
        OnActionExecute?.Invoke();
    }
}
