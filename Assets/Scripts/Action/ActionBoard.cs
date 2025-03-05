using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBoard : MonoBehaviour
{
    public UnityAction OnReserveExecuteAction;
    public UnityAction OnReserveExitAction;
    public UnityAction OnExecuteBattleAction;
    public UnityAction OnExitBattleAction;

    [SerializeField] private TalkPanel talkPanel;
    [SerializeField] private AttackPanel attackPanel;
    [SerializeField] private CommandPanel commandPanel;
    [SerializeField] private PouchPanel pouchPanel;
    [SerializeField] private BagPanel bagPanel;
    [SerializeField] private DeckPanel deckPanel;
    [SerializeField] private StatusPanel statusPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] private QuitPanel quitPanel;

    private Dictionary<ActionType, Panel> actionPanels;
    private Dictionary<EventType, UnityAction> executeActions;
    private Dictionary<EventType, UnityAction> exitActions;

    private EventType eventType;

    private void Start()
    {
        actionPanels = new Dictionary<ActionType, Panel>
        {
            { ActionType.Talk, talkPanel },
            { ActionType.Attack, attackPanel },
            { ActionType.Command, commandPanel },
            { ActionType.Pouch, pouchPanel },
            { ActionType.Bag, bagPanel },
            { ActionType.Deck, deckPanel },
            { ActionType.Status, statusPanel },
            { ActionType.Escape, escapePanel },
            { ActionType.Quit, quitPanel }
        };

        executeActions = new Dictionary<EventType, UnityAction>
        {
            { EventType.Reserve, OnReserveExecuteAction },
            { EventType.Battle, OnExecuteBattleAction }
        };

        exitActions = new Dictionary<EventType, UnityAction>
        {
            { EventType.Reserve, OnReserveExitAction },
            { EventType.Battle, OnExitBattleAction }
        };

        foreach (var panel in actionPanels.Values)
        {
            panel.OnActionExecute += ActionExecute;
            panel.OnActionExit += ActionExit;
        }
    }

    public void SetEventType(EventType type) => eventType = type;

    public void ChangeActionPanel(ActionType targetAction)
    {
        ClosePanel();
        if (actionPanels.TryGetValue(targetAction, out Panel panel))
        {
            panel.PanelOpen();
        }
    }

    public void ClosePanel()
    {
        foreach (var panel in actionPanels.Values)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void ChangeExecuteFlg(bool executeFlg)
    {
        foreach (var panel in actionPanels.Values)
        {
            panel.executeFlg = executeFlg;
        }
    }

    public void ActionExecute()
    {
        if (executeActions.TryGetValue(eventType, out UnityAction action))
        {
            action?.Invoke();
        }
    }

    public void ActionExit()
    {
        if (exitActions.TryGetValue(eventType, out UnityAction action))
        {
            action?.Invoke();
        }
    }
}
