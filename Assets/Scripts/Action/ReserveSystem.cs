using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd;

    [SerializeField] ActionIcon actionIconPrefab;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] GameObject actionListObject;
    [SerializeField] MessagePanel messagePanel;

    // ステート管理
    private ReserveState state;
    private ActionType activeAction = ActionType.Bag;
    private ActionIcon selectedAction;
    private List<ActionType> actionList = new List<ActionType>();
    private List<ActionIcon> actionIconList = new List<ActionIcon>();

    void Start()
    {
        actionList.Add(ActionType.Bag);
        actionList.Add(ActionType.Deck);
        actionList.Add(ActionType.Quit);
        state = ReserveState.Standby;
        transform.gameObject.SetActive(false);
    }

    public void SetState(ReserveState targetState)
    {
        state = targetState;
    }

    public void Update()
    {
        if (state == ReserveState.ActionSelection)
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index + 1) % actionList.Count; // 次のインデックスへ（リストの範囲を超えたら先頭へ）
                activeAction = actionList[index]; // 更新
                SelectAction(activeAction);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index - 1 + actionList.Count) % actionList.Count; // 前のインデックスへ（負の値を回避）
                activeAction = actionList[index]; // 更新
                SelectAction(activeAction);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = ReserveState.ActionSelected;
            }
        }
    }

    public void ReserveStart()
    {
        state = ReserveState.ActionSelection;
        transform.gameObject.SetActive(true);
        setActionList();
    }

    private void setActionList()
    {
        foreach (ActionType actionValue in actionList)
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionListObject.transform);
            actionIconList.Add(actionIcon);
            actionIcon.SetAction(actionValue);
            if (activeAction == actionValue)
            {
                actionBoard.ChangeActionPanel(actionValue);
            }
        }

        selectedAction = actionIconList.Count > 0 ? actionIconList[0] : null;

        if (selectedAction)
        {
            selectedAction.SetActive(true);
        }
    }

    private void SelectAction(ActionType selectAction)
    {
        actionBoard.ChangeActionPanel(selectAction);
        SelectActiveActionIcon(selectAction);
        activeAction = selectAction;
    }

    public void ExecuteAction()
    {
        switch (activeAction)
        {
            case ActionType.Bag:
                Debug.Log("Bag を開く処理を実行");
                break;

            case ActionType.Deck:
                Debug.Log("Deck を開く処理を実行");
                break;

            case ActionType.Escape:
                Debug.Log("Escape を実行");
                break;

            default:
                Debug.Log("未定義のアクションが選択されました");
                break;
        }

        // アクション実行後は、State を Standby に戻す
        state = ReserveState.ActionSelection;
    }

    public void ExitAction()
    {
        state = ReserveState.ActionSelection;
    }

    private void SelectActiveActionIcon(ActionType target)
    {
        // 現在選択中のアクションを非アクティブにする
        if (selectedAction != null)
        {
            selectedAction.SetActive(false);
        }

        // 対応するアクションアイコンを探してアクティブにする
        foreach (ActionIcon icon in actionIconList)
        {
            if (icon.type == activeAction)
            {
                selectedAction = icon;
                selectedAction.SetActive(false);
            }

            if (icon.type == target)
            {
                selectedAction = icon;
                selectedAction.SetActive(true);
            }
        }
    }

    public IEnumerator ResorveEnd()
    {
        state = ReserveState.Standby;
        yield return StartCoroutine(messagePanel.TypeDialog($"closed the back"));
        yield return new WaitForSeconds(1.0f);
        OnReserveEnd?.Invoke();
    }
}