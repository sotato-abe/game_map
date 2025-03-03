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

    [SerializeField] TalkPanel talkPanel;
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
        panelList.Add(talkPanel);
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
                talkPanel.PanelOpen();
                break;
            case ActionType.Attack:
                attackPanel.PanelOpen();
                break;
            case ActionType.Command:
                commandPanel.PanelOpen();
                break;
            case ActionType.Pouch:
                pouchPanel.PanelOpen();
                break;
            case ActionType.Bag:
                bagPanel.PanelOpen();
                break;
            case ActionType.Deck:
                deckPanel.PanelOpen();
                break;
            case ActionType.Escape:
                escapePanel.PanelOpen();
                break;
            case ActionType.Quit:
                quitPanel.PanelOpen();
                break;
        }
    }

    public void ClosePanel()
    {
        talkPanel.gameObject.SetActive(false);
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        deckPanel.gameObject.SetActive(false);
        escapePanel.gameObject.SetActive(false);
        quitPanel.gameObject.SetActive(false);
    }

    public void ChangeExecuteFlg(bool executeFlg)
    {
        foreach (Panel panel in panelList)
        {
            panel.executeFlg = executeFlg;
        }
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
