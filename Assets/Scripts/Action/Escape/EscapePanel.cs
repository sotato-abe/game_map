using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EscapePanel : Panel
{
    public UnityAction OnActionExecute;
    public UnityAction OnActionExit;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField]

    public void Update()
    {
        if (isActive)
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
