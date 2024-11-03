using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBoard : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] ActionPanel actionPanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] ItemPanel itemPanel;
    Action action;

    public ItemPanel ItemPanel => itemPanel;

    public void Init()
    {
        action = 0;
    }

    public void changeDialogType(Action targetAction)
    {
        action = targetAction;
        switch (action)
        {
            case Action.Talk:
                SetTalkPanel();
                break;
            case Action.Attack:
                SetAttackPanel();
                break;
            case Action.Command:
                SetCommandPanel();
                break;
            case Action.Item:
                SetItemPanel();
                break;
            case Action.Escape:
                SetEscapeDialog();
                break;
        }
    }

    public void TargetSelection(bool targetDirection)
    {
        switch (action)
        {
            case Action.Talk:
                break;
            case Action.Attack:
                break;
            case Action.Command:
                commandPanel.SelectCommand(targetDirection);
                break;
            case Action.Item:
                itemPanel.SelectItem(targetDirection);
                break;
            case Action.Escape:
                break;
        }
    }

    private void SetTalkPanel()
    {
        Debug.Log("SetTalkPanel");
        itemPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(true);
    }

    private void SetAttackPanel()
    {
        Debug.Log("SetAttackPanel");
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
    }

    private void SetCommandPanel()
    {
        Debug.Log("SetCommandPanel");
        itemPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(true);
    }

    private void SetItemPanel()
    {
        Debug.Log("SetItemPanel");
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(true);
    }

    private void SetEscapeDialog()
    {
        Debug.Log("SetEscapeDialog");
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
    }

    public IEnumerator SetMessageText(string message)
    {
        yield return StartCoroutine(messagePanel.GetComponent<MessagePanel>().TypeDialog(message));
    }
}
