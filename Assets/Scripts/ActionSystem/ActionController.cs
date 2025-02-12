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
    [SerializeField] ActionBoard actionDialog;
    [SerializeField] GameObject actionList;

    // 選択中のアクション
    ActionIcon selectedAction;

    // アクションの個数
    int actoinNum = 0;
    public int selectedIndex = 0;

    private List<ActionIcon> actionIconList = new List<ActionIcon>();

    private void Awake()
    {
        // 一度初期化
    }

    public void ResetActionList()
    {
        actoinNum = 0;
        selectedIndex = 0;
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
        foreach (ActionType actionValue in System.Enum.GetValues(typeof(ActionType)))
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionList.transform);
            actionIconList.Add(actionIcon);
            actionIcon.SetAction(actionValue);
            actoinNum++;
        }

        // ★削除済みのオブジェクトを参照しないようにリセット
        selectedAction = actionIconList.Count > 0 ? actionIconList[0] : null;
        if (selectedAction)
        {
            selectedAction.SetActive(true);
        }
    }

    public void RemoveActionList()
    {
        actoinNum = 0;
        selectedIndex = 0;
        foreach (ActionIcon icon in actionIconList)
        {
            Destroy(icon.gameObject);
        }
        actionIconList.Clear();
    }

    public void SelectAction(bool prev)
    {
        // インデックスをループ処理
        selectedIndex = prev ? (selectedIndex + 1) % actoinNum : (selectedIndex - 1 + actoinNum) % actoinNum;

        Debug.Log($"SelectAction {prev}");

        // 現在の選択を解除
        selectedAction?.SetActive(false);

        // 新しい選択を設定
        selectedAction = actionIconList[selectedIndex];
        selectedAction?.SetActive(true);
        actionDialog.changeDialogType((ActionType)selectedIndex);
    }
}
