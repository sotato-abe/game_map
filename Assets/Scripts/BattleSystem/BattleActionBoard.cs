using System.Collections.Generic;
using UnityEngine;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class BattleActionBoard : MonoBehaviour
{
    [SerializeField] private AttackPanel attackPanel;
    [SerializeField] private CommandPanel commandPanel;
    [SerializeField] private PouchPanel pouchPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] private ActionIcon attackIcon;
    [SerializeField] private ActionIcon commandIcon;
    [SerializeField] private ActionIcon pouchIcon;
    [SerializeField] private ActionIcon escapeIcon;

    private Dictionary<ActionType, Panel> actionPanels;
    private Dictionary<ActionType, ActionIcon> actionIcons;
    private List<ActionType> actionTypeList;
    private int currentIndex = 0;

    private void Start()
    {
        actionPanels = new Dictionary<ActionType, Panel>
        {
            { ActionType.Attack, attackPanel },
            { ActionType.Command, commandPanel },
            { ActionType.Pouch, pouchPanel },
            { ActionType.Escape, escapePanel },
        };

        actionIcons = new Dictionary<ActionType, ActionIcon>
        {
            { ActionType.Attack, attackIcon },
            { ActionType.Command, commandIcon },
            { ActionType.Pouch, pouchIcon },
            { ActionType.Escape, escapeIcon },
        };

        actionTypeList = new List<ActionType>(actionPanels.Keys);

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
        ActionType targetAction = actionTypeList[currentIndex];

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
            kvp.Value.SetActive(kvp.Key == targetAction); // 選択状態を表示
        }
    }

    public void SetEnemyListToPanel(List<Battler> enemyBattlers)
    {
        escapePanel.SetEnemyList(enemyBattlers);
    }
}
