using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd;

    [SerializeField] ActionBoard actionBoard;
    [SerializeField] ActionController actionController;
    [SerializeField] MessagePanel messagePanel;

    // ステート管理
    private ReserveState state;
    private ActionType activeAction = ActionType.Bag;
    private List<ActionType> actionList = new List<ActionType>();

    void Start()
    {
        actionList.Add(ActionType.Bag);
        actionList.Add(ActionType.Deck);
        actionList.Add(ActionType.Escape);
        state = ReserveState.Standby;
        transform.gameObject.SetActive(false);
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
                actionController.ChangeActionPanel(activeAction);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index - 1 + actionList.Count) % actionList.Count; // 前のインデックスへ（負の値を回避）
                activeAction = actionList[index]; // 更新
                actionController.ChangeActionPanel(activeAction);
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
        actionController.ResetActionList(actionList);
        transform.gameObject.SetActive(true);
    }

    public IEnumerator ResorveEnd()
    {
        state = ReserveState.Standby; 
        yield return StartCoroutine(messagePanel.TypeDialog($"closed the back"));
        yield return new WaitForSeconds(1.0f);
        actionController.CloseAction();
        OnReserveEnd?.Invoke();
    }
}