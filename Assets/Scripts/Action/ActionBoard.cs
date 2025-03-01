using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ActionBoard : MonoBehaviour
{
    public UnityAction OnActionExecute;
    public UnityAction OnActionExit;

    [SerializeField] AttackPanel attackPanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] PouchPanel pouchPanel;
    [SerializeField] BagPanel bagPanel;
    [SerializeField] DeckPanel deckPanel;
    [SerializeField] EscapePanel escapePanel;
    [SerializeField] QuitPanel quitPanel;
    ActionType action;

    private void Start()
    {
        escapePanel.OnActionExecute += ActionExecute;
        escapePanel.OnActionExit += ActionExit;
    }

    public void Init()
    {
        Debug.Log("init");
        action = 0;

    }

    public void ChangeActionPanel(ActionType targetAction)
    {
        action = targetAction;
        ResetPanel();
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
            case ActionType.Pouch:
                SetPouchPanel();
                break;
            case ActionType.Bag:
                SetBagPanel();
                break;
            case ActionType.Deck:
                SetDeckPanel();
                break;
            case ActionType.Escape:
                SetEscapeDialog();
                break;
            case ActionType.Quit:
                SetQuitDialog();
                break;
        }
    }

    private void ResetPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        deckPanel.gameObject.SetActive(false);
        escapePanel.ClosePanel();
        quitPanel.ClosePanel();
    }

    private void SetTalkPanel()
    {
    }

    private void SetAttackPanel()
    {
        attackPanel.gameObject.SetActive(true);
        attackPanel.PanelOpen();
    }

    private void SetCommandPanel()
    {
        commandPanel.gameObject.SetActive(true);
        commandPanel.PanelOpen();
    }

    private void SetPouchPanel()
    {
        pouchPanel.gameObject.SetActive(true);
        pouchPanel.PanelOpen();
    }

    private void SetBagPanel()
    {
        bagPanel.gameObject.SetActive(true);
        bagPanel.PanelOpen();
    }

    private void SetDeckPanel()
    {
        deckPanel.gameObject.SetActive(true);
        deckPanel.PanelOpen();
    }

    private void SetEscapeDialog()
    {
        escapePanel.gameObject.SetActive(true);
        escapePanel.PanelOpen();
    }

    private void SetQuitDialog()
    {
        quitPanel.gameObject.SetActive(true);
        quitPanel.PanelOpen();
    }

    public void CloseActionPanel()
    {
        ResetPanel();
    }

    public void ActionExecute()
    {
        Debug.Log("ActionExecute");
        OnActionExecute?.Invoke();
    }

    public void ActionExit()
    {
        Debug.Log("ActionExit");
        OnActionExit?.Invoke();
    }
}
