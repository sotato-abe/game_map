using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EscapePanel : Panel
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AttackSystem attackSystem;

    public void Update()
    {
        if (executeFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Escape();
            }
        }
    }

    public void SetEscapePanel()
    {

    }

    private void Escape()
    {
        if (executeFlg)
        {
            isActive = false;
            attackSystem.ExecutePlayerEscape();
        }
    }
}
