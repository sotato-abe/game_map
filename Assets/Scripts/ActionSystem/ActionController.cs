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
    [SerializeField] GameObject actionList;

    ActionType selectedAction;

    int actoinNum = 0;

    private List<ActionIcon> actionIconList = new List<ActionIcon>();

    private void Awake()
    {
        SetActionList();
    }

    public void SetActionList()
    {
        foreach (ActionType actionValue in System.Enum.GetValues(typeof(ActionType)))
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionList.transform);
            actionIcon.SetAction(actionValue);
            actoinNum++;
        }

    }
}
