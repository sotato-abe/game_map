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

    // 選択中のアクション
    ActionIcon selectedAction;

    // アクションの個数
    int actoinNum = 0;
    int selectedIndex = 0;

    private List<ActionIcon> actionIconList = new List<ActionIcon>();

    private void Awake()
    {
        // 一度初期化
        SetActionList();
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

    public void SelectAction(bool prev)
    {
        int targetIndex = selectedIndex;

        if (prev)
        {
            targetIndex++; // 右（次の要素）に進む
            if (targetIndex >= actoinNum) targetIndex = 0; // 最後を超えたら最初に戻る
        }
        else
        {
            targetIndex--; // 左（前の要素）に進む
            if (targetIndex < 0) targetIndex = actoinNum - 1; // 最初より前なら最後に戻る
        }

        if (targetIndex != selectedIndex)
        {
            Debug.Log($"SelectAction{prev}");
            // ★削除されていたらスキップ
            if (selectedAction != null)
            {
                selectedAction.SetActive(false);
            }

            selectedIndex = targetIndex; // 選択されたインデックスを更新
            selectedAction = actionIconList[selectedIndex]; // 新しい選択アイテムを取得

            if (selectedAction != null)
            {
                selectedAction.SetActive(true);
            }
        }
    }

}
