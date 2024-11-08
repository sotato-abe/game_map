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
    [SerializeField] TurnOrderSystem turnOrderSystem;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] ActionPanel actionPanel;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionPanel.Init();
    }

    public void Update()
    {
        if (state == BattleState.ActionSelection)
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
                StartCoroutine(SetBattleState(BattleState.ActionExecution));
            }
        }
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle(player, enemy));
        battleCanvas.gameObject.SetActive(true);
        actionPanel.SetActionValidity(0.2f);
        enemyUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Damn,,")); // TODO : キャラクターメッセージリストから取得する。
        state = BattleState.TurnWait;
        List<Battler> battlers = new List<Battler> { player, enemy };
        turnOrderSystem.SetUpBattlerTurns(battlers);
        turnOrderSystem.SetActive(true);
    }

    public IEnumerator SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        actionBoard.changeDialogType(Action.Talk);
        yield return StartCoroutine(actionBoard.SetMessageText($"{enemy.Base.Name} is coming!!"));
    }

    public IEnumerator SetBattleState(BattleState newState)
    {
        state = newState;
        switch (state)
        {
            case BattleState.Start:
                break;
            case BattleState.TurnWait:
                HandleTurnWait();
                break;
            case BattleState.ActionSelection:
                HandleActionSelection();
                break;
            case BattleState.ActionExecution:
                yield return StartCoroutine(HandleActionExecution());
                break;
            case BattleState.BattleResult:
                // yield return StartCoroutine(BattleResult(playerUnit, enemyUnit));
                break;
            case BattleState.BattleOver:
                BattleEnd();
                break;
        }
    }

    void HandleTurnWait()
    {
        turnOrderSystem.SetActive(true);
        actionPanel.SetActionValidity(0.2f);
    }

    void HandleActionSelection()
    {
        actionPanel.SetActionValidity(1.0f);
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
                yield return StartCoroutine(EscapeTurn());
                break;
        }
        actionPanel.SetActionValidity(1f);
        StartCoroutine(SetBattleState(BattleState.TurnWait));
    }

    public IEnumerator TalkTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText("The player tried talking to him, but he didn't respond."));
    }

    public IEnumerator AttackTurn()
    {
        state = BattleState.ActionExecution;
        yield return StartCoroutine(AttackAction(playerUnit, enemyUnit));
    }

    public IEnumerator CommandTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("I'm serious")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText("Implant activation start... Activation"));
    }

    public IEnumerator ItemTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("Take this!")); // TODO : キャラクターメッセージリストから取得する。
        actionBoard.ItemPanel.UseItem();
        yield return StartCoroutine(actionBoard.SetMessageText("The player fished through his backpack but found nothing"));
    }

    public IEnumerator EscapeTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(enemyUnit.SetTalkMessage("Wait!!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Let's run for it here")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText("Player is trying to escape"));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SetBattleState(BattleState.BattleOver));
    }

    //AtackManagerに切り離す
    private IEnumerator AttackAction(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        int damage = targetUnit.Battler.TakeDamage(sourceUnit.Battler);
        targetUnit.UpdateUI();
        targetUnit.SetMotion(BattleUnit.Motion.Jump);
        StartCoroutine(sourceUnit.SetTalkMessage("I'm gonna crush you")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(targetUnit.SetTalkMessage("Auch!!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(actionBoard.SetMessageText($"{targetUnit.Battler.Base.Name} take {damage} dameged by {sourceUnit.Battler.Base.Name}"));

        if (targetUnit.Battler.Life <= 0)
        {
            StartCoroutine(SetBattleState(BattleState.BattleResult));
            yield return StartCoroutine(BattleResult(sourceUnit, targetUnit));
        }
    }

    public IEnumerator BattleResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(targetUnit.SetTalkMessage("You'll regret this!!")); // TODO : キャラクターメッセージリストから取得する。
        targetUnit.SetMotion(BattleUnit.Motion.Jump);
        yield return StartCoroutine(actionBoard.SetMessageText($"{targetUnit.Battler.Base.Name} walked away\n{sourceUnit.Battler.Base.Name} win"));

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
            yield return StartCoroutine(actionBoard.SetMessageText(resultitemessage));
        }
        else
        {
            yield return StartCoroutine(actionBoard.SetMessageText("No items were found on the enemy."));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(SetBattleState(BattleState.BattleOver));
    }

    public void BattleEnd()
    {
        OnBattleEnd?.Invoke();
    }
}
