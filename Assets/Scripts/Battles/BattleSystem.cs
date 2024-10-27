using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public UnityAction OnBattleEnd;
    [SerializeField] BattleCanvas battleCanvas;
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] BattleActionDialog actionDialog;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionDialog.Init();
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = BattleState.Start;
        SetupBattle(player, enemy);
        battleCanvas.gameObject.SetActive(true);
        actionDialog.SetActionValidity(1f);
        enemyUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Damn,,")); // TODO : キャラクターメッセージリストから取得する。
        state = BattleState.ActionSelection; // 仮に本来はターンコントロ－ラーに入る
        StartCoroutine(SetBattleState(BattleState.ActionSelection));
    }

    public void SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        messageDialog.changeDialogType(BattleAction.Talk);
        StartCoroutine(SetDialogMessage($"{enemy.Base.Name} is coming!!"));
    }

    public IEnumerator SetBattleState(BattleState newState)
    {
        state = newState;
        switch (state)
        {
            case BattleState.Start:
                break;
            case BattleState.TurnWait:
                break;
            case BattleState.ActionSelection:
                HandleActionSelection();
                break;
            case BattleState.ActionExecution:
                yield return StartCoroutine(HandleActionExecution());
                break;
            case BattleState.BattleResult:
                yield return StartCoroutine(BattleResult(playerUnit, enemyUnit));
                break;
            case BattleState.BattleOver:
                BattleEnd();
                break;
        }
    }

    void HandleActionSelection()
    {
    }

    public IEnumerator HandleActionExecution()
    {
        Debug.Log("ActionExecution");
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
        actionDialog.SetActionValidity(1f);
        StartCoroutine(SetBattleState(BattleState.ActionSelection));
    }

    public IEnumerator TalkTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("The player tried talking to him, but he didn't respond."));
    }

    public IEnumerator AttackTurn()
    {
        state = BattleState.ActionExecution;
        yield return StartCoroutine(AttackAction(playerUnit, enemyUnit));
        if (state != BattleState.BattleOver)
        {
            yield return StartCoroutine(AttackAction(enemyUnit, playerUnit));
        }
    }

    public IEnumerator CommandTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I'm serious")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("Implant activation start... Activation"));
    }

    public IEnumerator ItemTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I wonder if he had any itemsitemsitems")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("The player fished through his backpack but found nothing."));
    }

    public IEnumerator RunTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(enemyUnit.SetTalkMessage("Wait!!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Let's run for it here")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage("Player is trying to escape."));
        StartCoroutine(SetBattleState(BattleState.BattleOver));
    }

    //AtackManagerに切り離す
    private IEnumerator AttackAction(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(sourceUnit.SetTalkMessage("I'm gonna crush you")); // TODO : キャラクターメッセージリストから取得する。
        int damage = targetUnit.Battler.TakeDamage(sourceUnit.Battler);
        targetUnit.UpdateUI();
        targetUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(targetUnit.SetTalkMessage("Auch!!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(SetDialogMessage($"{targetUnit.Battler.Base.Name} take {damage} dameged by {sourceUnit.Battler.Base.Name}"));

        if (targetUnit.Battler.Life <= 0)
        {
            StartCoroutine(SetBattleState(BattleState.BattleResult));
        }
    }

    public IEnumerator BattleResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(targetUnit.SetTalkMessage("You'll regret this!!")); // TODO : キャラクターメッセージリストから取得する。
        targetUnit.SetMotion(BattleUnit.Motion.Jump);
        yield return StartCoroutine(SetDialogMessage($"{targetUnit.Battler.Base.Name} walked away\n{sourceUnit.Battler.Base.Name} win"));

        List<Item> targetItems = targetUnit.Battler.Base.Items;
        if (targetItems != null && targetItems.Count > 0)
        {
            // ランダムにアイテムを取得（例: 2つ取得）
            int itemsToAward = Mathf.Min(2, targetItems.Count); // 最大2個
            List<Item> awardedItems = new List<Item>();

            for (int i = 0; i < itemsToAward; i++)
            {
                Item randomItem = targetItems[Random.Range(0, targetItems.Count)];
                awardedItems.Add(randomItem);
                sourceUnit.Battler.AddItemToInventory(randomItem); // プレイヤーのインベントリに追加
            }

            string resultitemessage = $"{sourceUnit.Battler.Base.Name} obtained ";

            // 獲得したアイテムを表示
            foreach (Item item in awardedItems)
            {
                resultitemessage += $"{item.Base.Name},";
            }
            yield return StartCoroutine(SetDialogMessage(resultitemessage));
        }
        else
        {
            yield return StartCoroutine(SetDialogMessage("No items were found on the enemy."));
        }

        yield return new WaitForSeconds(1f);
        BattleEnd();
    }

    public void BattleEnd()
    {
        StartCoroutine(SetBattleState(BattleState.BattleOver));
        OnBattleEnd?.Invoke();
    }

    public IEnumerator SetDialogMessage(string message)
    {
        yield return messageDialog.TypeDialog(message);
    }
}
