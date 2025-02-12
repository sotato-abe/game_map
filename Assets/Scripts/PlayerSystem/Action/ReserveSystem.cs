using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public ReserveState state;
    public UnityAction OnReserveEnd;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] ActionController actionController;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] BattleUnit playerUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (state == ReserveState.ActionSelection)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                actionController.SelectAction(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                actionController.SelectAction(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                actionBoard.TargetSelection(true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                actionBoard.TargetSelection(false);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SetReserveState(ReserveState.ActionExecution));
            }
        }
    }

    public void ReserveStart(Battler player)
    {
        state = ReserveState.Start;
        actionBoard.changeDialogType(ActionType.Talk);
        actionController.ResetActionList();
        state = ReserveState.ActionSelection; // 仮に本来はターンコントロ－ラーに入る
        StartCoroutine(SetReserveState(ReserveState.ActionSelection));
        StartCoroutine(playerUnit.SetTalkMessage("let's see"));
        StartCoroutine(messagePanel.GetComponent<MessagePanel>().TypeDialog($"{playerUnit.Battler.Base.Name} open the back"));
        messagePanel.gameObject.SetActive(true);
    }

    public IEnumerator SetReserveState(ReserveState newState)
    {
        state = newState;
        switch (state)
        {
            case ReserveState.Start:
                break;
            case ReserveState.ActionSelection:
                HandleActionSelection();
                break;
            case ReserveState.ActionExecution:
                yield return StartCoroutine(HandleActionExecution());
                break;
        }
    }

    void HandleActionSelection()
    {
    }

    public IEnumerator HandleActionExecution()
    {
        ActionType action = (ActionType)actionController.selectedIndex;

        switch (action)
        {
            case ActionType.Talk:
                yield return StartCoroutine(TalkTurn());
                break;
            case ActionType.Attack:
                yield return StartCoroutine(AttackTurn());
                break;
            case ActionType.Command:
                yield return StartCoroutine(CommandTurn());
                break;
            case ActionType.Item:
                yield return StartCoroutine(ItemTurn());
                break;
            case ActionType.Escape:
                yield return StartCoroutine(ResorveEnd());
                break;
        }
        StartCoroutine(SetReserveState(ReserveState.ActionSelection));
    }

    public IEnumerator TalkTurn()
    {
        state = ReserveState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText("The player tried talking to him, but he didn't respond."));
    }

    public IEnumerator AttackTurn()
    {
        state = ReserveState.ActionExecution;
        yield return null;
    }

    public IEnumerator CommandTurn()
    {
        state = ReserveState.ActionExecution;
        yield return StartCoroutine(actionBoard.SetMessageText("Implant activation start... Activation"));
    }

    public IEnumerator ItemTurn()
    {
        state = ReserveState.ActionExecution;
        playerUnit.SetMotion(MotionType.Rotate);
        StartCoroutine(playerUnit.SetTalkMessage("Take this!")); // TODO : キャラクターメッセージリストから取得する。
        actionBoard.ItemPanel.UseItem();
        yield return StartCoroutine(actionBoard.SetMessageText("The player fished through his backpack but found nothing"));
    }

    public IEnumerator ResorveEnd()
    {
        state = ReserveState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("all right")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText($"{playerUnit.Battler.Base.Name} closed the back"));
        yield return new WaitForSeconds(1.0f);
        actionController.RemoveActionList();
        OnReserveEnd?.Invoke();
    }
}
