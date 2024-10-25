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

    public IEnumerator AttackTurn()
    {
        state = State.ActionExecution;
        yield return StartCoroutine(AttackAction(playerUnit, enemyUnit));
        if (state != State.BattleOver)
        {
            yield return StartCoroutine(AttackAction(enemyUnit, playerUnit));
        }
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
        BattleEnd();
    }

    private IEnumerator AttackAction(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(sourceUnit.SetTalkMessage("I'm gonna crush you")); // TODO : キャラクターメッセージリストから取得する。
        int damage = targetUnit.Battler.TakeDamege(sourceUnit.Battler);
        targetUnit.UpdateUI();
        targetUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(targetUnit.SetTalkMessage("Auch!!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage($"{targetUnit.Battler.Base.Name} take {damage} dameged by {sourceUnit.Battler.Base.Name}"));

        if (targetUnit.Battler.Life <= 0)
        {
            yield return StartCoroutine(BattleResult(sourceUnit));
        }
    }

    public IEnumerator BattleResult(BattleUnit sourceUnit)
    {
        yield return StartCoroutine(SetDialogMessage($"{sourceUnit.Battler.Base.Name} win"));
        yield return new WaitForSeconds(2f);
        BattleEnd();
    }

    public void BattleEnd()
    {
        state = State.BattleOver;
        OnBattleEnd?.Invoke();
    }

    public IEnumerator SetDialogMessage(string message)
    {
        yield return messageDialog.TypeDialog(message);
    }
}
