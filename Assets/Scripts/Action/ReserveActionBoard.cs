using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// BattlePanelとIconの表示と切り替えを管理するクラス
public class ReserveActionBoard : MonoBehaviour
{
    public UnityAction OnReserveEnd;
    [SerializeField] private BagPanel bagPanel;
    [SerializeField] private StoragePanel storagePanel;
    [SerializeField] private StatusPanel statusPanel;
    [SerializeField] private ActionIcon bagIcon;
    [SerializeField] private ActionIcon storageIcon;
    [SerializeField] private ActionIcon statusIcon;

    private Dictionary<ActionType, Panel> actionPanels;
    private Dictionary<ActionType, ActionIcon> actionIcons;
    private List<ActionType> actionTypeList;
    private int currentIndex = 0;

    private void Start()
    {
        actionPanels = new Dictionary<ActionType, Panel>
        {
            { ActionType.Bag, bagPanel },
            { ActionType.Storage, storagePanel },
            { ActionType.Status, statusPanel },
        };

        actionIcons = new Dictionary<ActionType, ActionIcon>
        {
            { ActionType.Bag, bagIcon },
            { ActionType.Storage, storageIcon },
            { ActionType.Status, statusIcon },
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + actionTypeList.Count) % actionTypeList.Count;
            ChangeActionPanel();
        }
        // if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        // {
        //     transform.gameObject.SetActive(false);
        //     OnReserveEnd?.Invoke();
        // }
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
}
