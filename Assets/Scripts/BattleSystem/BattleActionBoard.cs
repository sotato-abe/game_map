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

    private Dictionary<BattleActionType, Panel> actionPanels;
    private Dictionary<BattleActionType, ActionIcon> actionIcons;
    private List<BattleActionType> actionTypeList;
    private BattleActionType currentAction = BattleActionType.Attack;

    private void Start()
    {
        actionPanels = new Dictionary<BattleActionType, Panel>
        {
            { BattleActionType.Attack, attackPanel },
            { BattleActionType.Command, commandPanel },
            { BattleActionType.Pouch, pouchPanel },
            { BattleActionType.Escape, escapePanel },
        };

        actionIcons = new Dictionary<BattleActionType, ActionIcon>
        {
            { BattleActionType.Attack, attackIcon },
            { BattleActionType.Command, commandIcon },
            { BattleActionType.Pouch, pouchIcon },
            { BattleActionType.Escape, escapeIcon },
        };

        actionTypeList = new List<BattleActionType>(actionPanels.Keys);

        ChangeActiveIcon();
        ChangeActionPanel();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAction = BattleActionType.Attack;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentAction = BattleActionType.Command;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAction = BattleActionType.Pouch;
                ChangeActiveIcon();
                ChangeActionPanel();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentAction = BattleActionType.Escape;
                ChangeActiveIcon();
                ChangeActionPanel();
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

    public void SetEnemyListToPanel(List<Battler> enemyBattlers)
    {
        escapePanel.SetEnemyList(enemyBattlers);
    }
}
