using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    [SerializeField] AttackSystem attackSystem;

    public void Update()
    {
        if (executeFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TalkExecute();
            }
        }
    }

    private void TalkExecute()
    {
        attackSystem.ExecutePlayerTalk();
    }
}
