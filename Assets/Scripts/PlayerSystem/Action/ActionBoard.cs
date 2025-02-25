using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBoard : MonoBehaviour
{
    [SerializeField] AttackPanel attackPanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] public PouchPanel pouchPanel;
    ActionType action;

    public void Init()
    {
        action = 0;
    }

    public void changeActionPanel(ActionType targetAction)
    {
        action = targetAction;
        switch (action)
        {
            case ActionType.Talk:
                SetTalkPanel();
                break;
            case ActionType.Attack:
                SetAttackPanel();
                break;
            case ActionType.Command:
                SetCommandPanel();
                break;
            case ActionType.Item:
                SetPouchPanel();
                break;
            case ActionType.Escape:
                SetEscapeDialog();
                break;
        }
    }

    public void TargetSelection(bool targetDirection)
    {
        switch (action)
        {
            case ActionType.Talk:
                break;
            case ActionType.Attack:
                break;
            case ActionType.Command:
                commandPanel.SelectCommand(targetDirection);
                break;
            case ActionType.Item:
                pouchPanel.SelectItem(targetDirection);
                break;
            case ActionType.Escape:
                break;
        }
    }

    private void SetTalkPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
    }

    private void SetAttackPanel()
    {
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        attackPanel.gameObject.SetActive(true);
    }

    private void SetCommandPanel()
    {
        attackPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(true);
    }

    private void SetPouchPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(true);
    }

    private void SetEscapeDialog()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
    }

    public void CloseActionPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
    }
}
