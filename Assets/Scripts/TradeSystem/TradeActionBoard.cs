using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class TradeActionBoard : MonoBehaviour
{
    public UnityAction OnTradeEnd;
    [SerializeField] private TalkPanel talkPanel;
    [SerializeField] private ShopPanel shopPanel;
    [SerializeField] private LabPanel labPanel;
    [SerializeField] private ActionIcon talkIcon;
    [SerializeField] private ActionIcon shopIcon;
    [SerializeField] private ActionIcon labIcon;
    [SerializeField] private ActionIcon quitIcon;

    private Dictionary<TradeActionType, Panel> actionPanels;
    private Dictionary<TradeActionType, ActionIcon> actionIcons;
    private List<TradeActionType> actionTypeList;
    private TradeActionType currentAction = TradeActionType.Talk;

    private void Start()
    {
        actionPanels = new Dictionary<TradeActionType, Panel>
        {
            { TradeActionType.Talk, talkPanel },
            { TradeActionType.Shop, shopPanel },
            { TradeActionType.Lab, labPanel },
        };

        actionIcons = new Dictionary<TradeActionType, ActionIcon>
        {
            { TradeActionType.Talk, talkIcon },
            { TradeActionType.Shop, shopIcon },
            { TradeActionType.Lab, labIcon },
            { TradeActionType.Quit, quitIcon },
        };

        actionTypeList = new List<TradeActionType>(actionPanels.Keys);

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAction = TradeActionType.Talk;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentAction = TradeActionType.Shop;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAction = TradeActionType.Lab;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentAction = TradeActionType.Quit;
                ChangeActiveIcon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == TradeActionType.Quit)
            {
                Debug.Log("Trade ended.");
                OnTradeEnd?.Invoke();
            }
        }
    }

    private void ChangeActiveIcon()
    {
        foreach (var kvp in actionIcons)
        {
            kvp.Value.SetActive(kvp.Key == currentAction); // 選択状態を表示
        }
    }

    private void ChangeActionPanel()
    {
        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == currentAction)
            {
                kvp.Value.PanelOpen();
            }
            else
            {
                kvp.Value.ClosePanel();
            }
        }
    }
}
