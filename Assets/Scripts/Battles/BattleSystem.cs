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
        TurnWait,
        ActionSelection,
        ActionExecution,
        BattleResult,
        BattleOver,
    }

    State state;
    public UnityAction OnBattleEnd;
    [SerializeField] BattleCanvas battleCanvas;
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] BattleActionDialog actionDialog;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] FieldInfoSystem fieldInfoSystem;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionDialog.Init();
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = State.Start;
        SetupBattle(player, enemy);
        battleCanvas.gameObject.SetActive(true);
        enemyUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Damn,,")); // TODO : キャラクターメッセージリストから取得する。
        state = State.ActionSelection;
    }

    public void SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        StartCoroutine(SetDialogMessage($"{enemy.Base.Name} is coming!!"));
    }

    private void Update()
    {
        switch (state)
        {
            case State.TurnWait:
                break;
            case State.ActionSelection:
                HandleActionSelection();
                break;
            case State.ActionExecution:
                break;
            case State.BattleResult:
                break;
            case State.BattleOver:
                break;
        }
    }

    void HandleActionSelection()
    {
        actionDialog.HandleUpdate();
    }

    public IEnumerator ActionExecution()
    {
        state = State.ActionExecution;
        BattleAction action = (BattleAction)actionDialog.selectedIndex;

        switch (action)
        {
            case BattleAction.Talk:
                yield return StartCoroutine(TalkTurn());
                break;
            case BattleAction.Attack:
                yield return StartCoroutine(AttackTurn());
                break;
            case BattleAction.Command:
                yield return StartCoroutine(CommandTurn());
                break;
            case BattleAction.Item:
                yield return StartCoroutine(ItemTurn());
                break;
            case BattleAction.Run:
                yield return StartCoroutine(RunTurn());
                break;
        }
        state = State.ActionSelection;
    }

    public IEnumerator TalkTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("The player tried talking to him, but he didn't respond."));
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

    public IEnumerator CommandTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I'm serious")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("Implant activation start... Activation"));
    }

    public IEnumerator ItemTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I wonder if he had any itemsitemsitems")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("The player fished through his backpack but found nothing."));
    }

    public IEnumerator RunTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(enemyUnit.SetTalkMessage("Wait!!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Let's run for it here")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("Player is trying to escape."));
        BattleEnd();
    }

    //AtackManagerに切り離す
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
            StartCoroutine(targetUnit.SetTalkMessage("You'll regret this!!")); // TODO : キャラクターメッセージリストから取得する。
            enemyUnit.SetMotion(BattleUnit.Motion.Jump);
            yield return StartCoroutine(SetDialogMessage($"{targetUnit.Battler.Base.Name} walked away\n{sourceUnit.Battler.Base.Name} win"));
            yield return new WaitForSeconds(2f);
            BattleEnd();
        }
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
