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
    [SerializeField] ActionPanel actionPanel;
    [SerializeField] BattleUnit playerUnit;

    public ActionPanel ActionPanel => actionPanel;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionPanel.Init();
    }

    public void Update()
    {
        if (state == ReserveState.ActionSelection)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                actionPanel.SetAction(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                actionPanel.SetAction(false);
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
                actionPanel.SetActionValidity(0.2f);
                StartCoroutine(SetReserveState(ReserveState.ActionExecution));
            }
        }
    }

    public void ReserveStart(Battler player)
    {
        state = ReserveState.Start;
        actionBoard.changeDialogType(Action.Talk);
        // StartCoroutine(actionBoard.SetMessageText("Sola fished through the bag."));
        actionBoard.changeDialogType(Action.Talk);
        actionPanel.SetActionValidity(1f);
        state = ReserveState.ActionSelection; // 仮に本来はターンコントロ－ラーに入る
        StartCoroutine(SetReserveState(ReserveState.ActionSelection));
    }

    public void SetupReserve(Battler player)
    {
        actionBoard.changeDialogType(Action.Talk);
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
        Action action = (Action)actionPanel.selectedIndex;

        switch (action)
        {
            case Action.Talk:
                yield return StartCoroutine(TalkTurn());
                break;
            case Action.Attack:
                yield return StartCoroutine(AttackTurn());
                break;
            case Action.Command:
                yield return StartCoroutine(CommandTurn());
                break;
            case Action.Item:
                yield return StartCoroutine(ItemTurn());
                break;
            case Action.Escape:
                yield return StartCoroutine(ResorveEnd());
                break;
        }
        actionPanel.SetActionValidity(1f);
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
        actionBoard.ItemPanel.UseItem();
        yield return StartCoroutine(actionBoard.SetMessageText("The player fished through his backpack but found nothing"));
    }

    public IEnumerator ResorveEnd()
    {
        state = ReserveState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("all right")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText("Sola closed the back"));
        yield return new WaitForSeconds(1.0f);
        OnReserveEnd?.Invoke();
    }
}
