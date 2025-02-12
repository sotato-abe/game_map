using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBoard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AttackPanel attackPanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] ItemPanel itemPanel;
    ActionType action;

    public ItemPanel ItemPanel => itemPanel;

    public void Init()
    {
        action = 0;
    }

    public void changeDialogType(ActionType targetAction)
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
                SetItemPanel();
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
                itemPanel.SelectItem(targetDirection);
                break;
            case ActionType.Escape:
                break;
        }
    }

    private void SetTalkPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(true);
    }

    private void SetAttackPanel()
    {
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        attackPanel.gameObject.SetActive(true);
    }

    private void SetCommandPanel()
    {
        attackPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(true);
    }

    private void SetItemPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(true);
    }

    private void SetEscapeDialog()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
    }

    public IEnumerator SetMessageText(string message)
    {
        yield return StartCoroutine(messagePanel.GetComponent<MessagePanel>().TypeDialog(message));
    }
}
