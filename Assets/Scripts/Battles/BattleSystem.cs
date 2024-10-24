using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    enum State
    {
        Start,
        ActionSelection,
        ActionExecution,
        RunTurn,
        BattleOver,
    }

    State state;
    public UnityAction OnBattleEnd;
    [SerializeField] BattleCanvas battleCanvas;
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case State.Start:
                break;
            case State.ActionSelection:
                HandleActionSelection();
                break;
            case State.ActionExecution:
                break;
            case State.BattleOver:
                break;
        }
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = State.Start;
        SetupBattle(player, enemy);
        battleCanvas.gameObject.SetActive(true);
        enemyUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Damn,,")); // TODO : キャラクターメッセージリストから取得する。
    }

    public void SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        StartCoroutine(SetDialogMessage($"{enemy.Base.Name} is coming!!"));
    }

    void HandleActionSelection()
    {

    }

    public void AttackTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I'm gonna crush you")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(SetDialogMessage("The player is waving his arms around."));
    }

    public void CommandTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I'm serious")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(SetDialogMessage("Implant activation start... Activation"));
    }

    public void ItemTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I wonder if he had any itemsitemsitems")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(SetDialogMessage("The player fished through his backpack but found nothing."));
    }

    public void TalkTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(SetDialogMessage("The player tried talking to him, but he didn't respond."));
    }

    public IEnumerator RunTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(SetDialogMessage("Player is trying to escape."));
        StartCoroutine(enemyUnit.SetTalkMessage("Wait!!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(playerUnit.SetTalkMessage("Let's run for it here")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(BattleEnd());
    }

    public IEnumerator SetDialogMessage(string message)
    {
        yield return messageDialog.TypeDialog(message);
    }

    public IEnumerator BattleEnd()
    {
        state = State.BattleOver;
        yield return StartCoroutine(SetDialogMessage("The player braced himself."));
        OnBattleEnd?.Invoke();
    }
}
