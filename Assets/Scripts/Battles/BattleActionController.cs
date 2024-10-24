using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionController : MonoBehaviour
{
    SelectableText[] selectableTexts;
    [SerializeField] BattleSystem battleSystem;

    enum State
    {
        Attack,
        Item,
        Talk,
        Run
    }

    int selectedIndex;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        selectedIndex = 0;
        selectableTexts = GetComponentsInChildren<SelectableText>();
    }

    public void Reset()
    {
        selectedIndex = (int)State.Attack;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);

        for (int i = 0; i < selectableTexts.Length; i++)
        {
            selectableTexts[i].SetSelectedColor(selectedIndex == i);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelectedAction();
        }
    }

    void ExecuteSelectedAction()
    {
        switch ((State)selectedIndex)
        {
            case State.Attack:
                HandleAttackAction();
                break;
            case State.Item:
                HandleItemAction();
                break;
            case State.Talk:
                HandleTalkAction();
                break;
            case State.Run:
                HandleRunAction();
                break;
        }
    }

    void HandleAttackAction()
    {
        battleSystem.AttackTurn();
    }

    void HandleItemAction()
    {
        battleSystem.ItemTurn();
    }

    void HandleTalkAction()
    {
        battleSystem.TalkTurn();
    }

    void HandleRunAction()
    {
        StartCoroutine(battleSystem.RunTurn());
    }
}
