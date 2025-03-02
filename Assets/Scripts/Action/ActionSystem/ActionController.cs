using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// イベントごとに選択可能なアクションをアクションリストに表示する
// アクション選択を行う
// アクションごとにアクションダイヤログを切り替える
public class ActionController : MonoBehaviour
{
    [SerializeField] ActionIcon actionIconPrefab;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] GameObject actionListObject;
    [SerializeField] ReserveSystem reserveSystem;

    // 選択中のアクション
    private ActionIcon selectedAction;
    public ActionType activeAction;
    private List<ActionIcon> actionIconList = new List<ActionIcon>();
    private List<ActionType> actionList = new List<ActionType>();

    private void Awake()
    {
        // 一度初期化
        actionBoard.OnActionExecute += ActionExecute;
        actionBoard.OnActionExit += ActionExit;
    }

    public void ResetActionList(List<ActionType> actions)
    {
        actionList = actions;
        foreach (ActionIcon icon in actionIconList)
        {
            Destroy(icon.gameObject);
        }
        actionIconList.Clear();
        SetActionList();
    }

    public void SetActionList()
    {
        // TODO イベントによって選択できるアクションリストを変更する
        foreach (ActionType actionValue in actionList)
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionListObject.transform);
            actionIconList.Add(actionIcon);
            actionIcon.SetAction(actionValue);
        }

        // ★削除済みのオブジェクトを参照しないようにリセット
        selectedAction = actionIconList.Count > 0 ? actionIconList[0] : null;
        if (selectedAction)
        {
            selectedAction.SetActive(true);
        }
    }

    public void ChangeActionPanel(ActionType type)
    {
        actionBoard.ChangeActionPanel(type);
        SelectActiveActionIcon(type);
        activeAction = type;
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
            if (icon.type == activeAction) // GetActionType() はアクションタイプを取得する仮のメソッド
            {
                selectedAction = icon;
                selectedAction.SetActive(false);
            }

            if (icon.type == target) // GetActionType() はアクションタイプを取得する仮のメソッド
            {
                selectedAction = icon;
                selectedAction.SetActive(true);
            }
        }
    }

    public void ActionExecute()
    {
        Debug.Log("ActionController:ActionExecute");
        switch (activeAction)
        {
            case ActionType.Talk:
                break;
            case ActionType.Attack:
                break;
            case ActionType.Command:
                break;
            case ActionType.Pouch:
                break;
            case ActionType.Bag:
                break;
            case ActionType.Deck:
                break;
            case ActionType.Escape:
                StartCoroutine(EscapeExecute());
                break;
        }
    }

    public void ActionExit()
    {
        Debug.Log("ActionController:ActionExit");
        reserveSystem.SetState(ReserveState.ActionSelection);
    }

    public IEnumerator EscapeExecute()
    {
        Debug.Log("ActionController:EscapeExecute");
        StartCoroutine(reserveSystem.ResorveEnd());
        yield return null;
    }

    public void CloseAction()
    {
        // actionBoard.CloseActionPanel();
        RemoveActionList();
    }

    public void RemoveActionList()
    {
        foreach (ActionIcon icon in actionIconList)
        {
            Destroy(icon.gameObject);
        }
        actionIconList.Clear();
    }
}
