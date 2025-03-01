using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] ActionController actionController;
    [SerializeField] MessagePanel messagePanel;

    public UnityAction OnReserveEnd;

    private ActionType targetAction = ActionType.Bag;

    private List<ActionType> actionList = new List<ActionType>();

    void Start()
    {
        actionList.Add(ActionType.Bag);
        actionList.Add(ActionType.Deck);
        actionList.Add(ActionType.Escape);
        transform.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int index = actionList.IndexOf(targetAction); // 現在のtargetActionのインデックスを取得
            index = (index + 1) % actionList.Count; // 次のインデックスへ（リストの範囲を超えたら先頭へ）
            targetAction = actionList[index]; // 更新
            actionController.ChangeActionPanel(targetAction);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int index = actionList.IndexOf(targetAction); // 現在のtargetActionのインデックスを取得
            index = (index - 1 + actionList.Count) % actionList.Count; // 前のインデックスへ（負の値を回避）
            targetAction = actionList[index]; // 更新
            actionController.ChangeActionPanel(targetAction);
        }
    }

    public void ReserveStart()
    {
        actionController.ResetActionList(actionList);
        transform.gameObject.SetActive(true);
    }

    public IEnumerator ResorveEnd()
    {
        yield return StartCoroutine(messagePanel.TypeDialog($"closed the back"));
        yield return new WaitForSeconds(1.0f);
        actionController.CloseAction();
        OnReserveEnd?.Invoke();
    }
}