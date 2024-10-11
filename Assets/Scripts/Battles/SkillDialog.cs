using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillDialog : MonoBehaviour
{
    SelectableText[] selectableTexts;
    [SerializeField] BattleSystem battleSystem;
    enum State
    {
        Attack,
        Spell,
        Back
    }

    int selectedIndex;

    void Start()
    {
        selectedIndex = (int)State.Attack;
        transform.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Init();
    }

    public void Reset()
    {
        selectedIndex = (int)State.Attack;
    }

    private void Init()
    {
        Debug.Log("SkillDialogInit");
        selectableTexts = GetComponentsInChildren<SelectableText>();
        StartCoroutine(battleSystem.SetMessage("The player braced himself!!"));
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
            case State.Spell:
                HandleSpellAction();
                break;
            case State.Back:
                HandleBackAction();
                break;
        }
    }

    private void HandleAttackAction()
    {
        Debug.Log("Attack!!");
        StartCoroutine(battleSystem.SetMessage("Attack!!"));
    }

    void HandleSpellAction()
    {
        Debug.Log("Spell!!");
        StartCoroutine(battleSystem.SetMessage("Spell!!"));

    }

    void HandleBackAction()
    {
        Debug.Log("Back!!");
        battleSystem.ActionDialogOpen();
    }
}
