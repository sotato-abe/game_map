using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ActionBoard : MonoBehaviour
{
    public UnityAction OnReserveExecuteAction;
    public UnityAction OnReserveExitAction;
    public UnityAction OnExecuteBattleAction;
    public UnityAction OnExitBattleAction;

    [SerializeField] AttackPanel attackPanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] PouchPanel pouchPanel;
    [SerializeField] BagPanel bagPanel;
    [SerializeField] DeckPanel deckPanel;
    [SerializeField] EscapePanel escapePanel;
    [SerializeField] QuitPanel quitPanel;
    ActionType action;

    EventType eventType;

    private List<Panel> panelList = new List<Panel>();

    private void Start()
    {
        panelList.Add(attackPanel);
        panelList.Add(commandPanel);
        panelList.Add(pouchPanel);
        panelList.Add(bagPanel);
        panelList.Add(deckPanel);
        panelList.Add(escapePanel);
        panelList.Add(quitPanel);

        foreach (Panel panel in panelList)
        {
            panel.OnActionExecute += ActionExecute;
            panel.OnActionExit += ActionExit;
        }
    }

    public void Init()
    {
        Debug.Log("init");
        action = 0;
    }

    public void SetEventType(EventType type)
    {
        eventType = type;
    }

    public void ChangeActionPanel(ActionType targetAction)
    {
        action = targetAction;
        ClosePanel();
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

    public void ClosePanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        deckPanel.gameObject.SetActive(false);
        escapePanel.gameObject.SetActive(false);
        quitPanel.gameObject.SetActive(false);
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

    public void ActionExecute()
    {
        if (eventType == EventType.Reserve)
        {
            OnReserveExecuteAction?.Invoke();
        }
        else if (eventType == EventType.Battle)
        {
            OnExecuteBattleAction?.Invoke();
        }
    }

    public void ActionExit()
    {
        if (eventType == EventType.Reserve)
        {
            OnReserveExitAction?.Invoke();
        }
        else if (eventType == EventType.Battle)
        {
            OnExitBattleAction?.Invoke();
        }
    }
}
