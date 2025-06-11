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

    private Dictionary<TradeActionType, Panel> actionPanels;
    private Dictionary<TradeActionType, ActionIcon> actionIcons;
    private List<TradeActionType> actionTypeList;
    private int currentIndex = 0;

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
        };

        actionTypeList = new List<TradeActionType>(actionPanels.Keys);

        ChangeActionPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % actionTypeList.Count;
            ChangeActionPanel();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + actionTypeList.Count) % actionTypeList.Count;
            ChangeActionPanel();
        }
    }

    private void ChangeActionPanel()
    {
        TradeActionType targetAction = actionTypeList[currentIndex];

        foreach (var kvp in actionPanels)
        {
            if (kvp.Key == targetAction)
            {
                kvp.Value.PanelOpen();
            }
            else
            {
                kvp.Value.ClosePanel();
            }
        }

        foreach (var kvp in actionIcons)
        {
            kvp.Value.SetActive(kvp.Key == targetAction);
        }
    }
}
