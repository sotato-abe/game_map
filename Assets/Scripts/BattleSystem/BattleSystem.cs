using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


// バトルエンカウント処理はこっちに入れる
// バトルエンカウントは時間とフィールドの危険度を元にランダムで決まる。

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public UnityAction OnBattleEnd;
    [SerializeField] bool isAuto; // オート状態　TODO：全体のオート状態を受け取る
    [SerializeField] TurnOrderSystem turnOrderSystem;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] ActionController actionController;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] AttackSystem attackSystem;

    void Start()
    {
        transform.gameObject.SetActive(true);
        enemyUnit.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (state == BattleState.ActionSelection)
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
                StartCoroutine(SetBattleState(BattleState.ActionExecution));
            }
        }
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle(player, enemy));

        actionController.ResetActionList();

        playerUnit.SetMotion(MotionType.Jump);
        playerUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。
        enemyUnit.gameObject.SetActive(true);
        enemyUnit.SetMotion(MotionType.Jump);
        enemyUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。

        state = BattleState.TurnWait;

        List<Battler> battlers = new List<Battler> { player, enemy };
        turnOrderSystem.SetUpBattlerTurns(battlers);
    }

    public IEnumerator SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        actionBoard.changeActionPanel(ActionType.Talk);
        yield return StartCoroutine(messagePanel.TypeDialog($"{enemy.Base.Name} is coming!!"));
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
        turnOrderSystem.EndTurn();
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
            case ActionType.Pouch:
                yield return StartCoroutine(ItemTurn());
                break;
            case ActionType.Escape:
                yield return StartCoroutine(EscapeTurn());
                break;
        }
        StartCoroutine(SetBattleState(BattleState.TurnWait));
    }

    public IEnumerator TalkTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(playerUnit.SetTalkMessage("what's up")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(enemyUnit.SetTalkMessage("yeaeeehhhhhhhhh!!\nI'm gonna blow you away!")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(messagePanel.TypeDialog("The player tried talking to him, but he didn't respond."));
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
        yield return StartCoroutine(messagePanel.TypeDialog("Implant activation start... Activation"));
    }

    public IEnumerator ItemTurn()
    {
        state = BattleState.ActionExecution;
        playerUnit.SetMotion(MotionType.Rotate);
        StartCoroutine(playerUnit.SetTalkMessage("Take this!")); // TODO : キャラクターメッセージリストから取得する。
        actionBoard.pouchPanel.UseItem();
        yield return StartCoroutine(messagePanel.TypeDialog("The player fished through his backpack but found nothing"));
    }

    public IEnumerator EscapeTurn()
    {
        state = BattleState.ActionExecution;
        StartCoroutine(enemyUnit.SetTalkMessage("Wait!!")); // TODO : キャラクターメッセージリストから取得する。
        StartCoroutine(playerUnit.SetTalkMessage("Let's run for it here")); // TODO : キャラクターメッセージリストから取得する。
        yield return StartCoroutine(messagePanel.TypeDialog("Player is trying to escape"));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SetBattleState(BattleState.BattleOver));
    }

    //AtackManagerに切り離す
    private IEnumerator AttackAction(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        // 攻撃前のアニメーションやメッセージ
        StartCoroutine(sourceUnit.SetTalkMessage("I'm gonna crush you"));
        StartCoroutine(targetUnit.SetTalkMessage("Auch!!"));
        yield return StartCoroutine(messagePanel.TypeDialog($"{sourceUnit.Battler.Base.Name} attacks {targetUnit.Battler.Base.Name}!"));

        // 攻撃処理
        attackSystem.ExecuteAttack(sourceUnit, targetUnit);

        // ダメージ適用後の待機
        yield return new WaitForSeconds(0.5f);

        // HPが0以下ならバトル結果処理
        if (targetUnit.Battler.Life <= 0)
        {
            yield return StartCoroutine(BattleResult(sourceUnit, targetUnit));
        }
    }

    public IEnumerator BattleResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(targetUnit.SetTalkMessage("You'll regret this!!")); // TODO : キャラクターメッセージリストから取得する。
        targetUnit.SetMotion(MotionType.Jump);
        yield return StartCoroutine(messagePanel.TypeDialog($"{targetUnit.Battler.Base.Name} walked away\n{sourceUnit.Battler.Base.Name} win"));

        List<Item> targetItems = targetUnit.Battler.Base.Items;
        if (targetItems != null && targetItems.Count > 0)
        {
            // ランダムにアイテムを取得（例: 2つ取得）
            int itemsToAward = Mathf.Min(2, targetItems.Count); // 最大2個
            List<Item> awardedItems = new List<Item>();

            // アイテムリストをシャッフル
            List<Item> shuffledItems = new List<Item>(targetItems);
            for (int i = 0; i < shuffledItems.Count; i++)
            {
                Item temp = shuffledItems[i];
                int randomIndex = Random.Range(i, shuffledItems.Count);
                shuffledItems[i] = shuffledItems[randomIndex];
                shuffledItems[randomIndex] = temp;
            }

            // シャッフルされたリストから最大2つを選択
            for (int i = 0; i < itemsToAward; i++)
            {
                Item randomItem = shuffledItems[i];
                awardedItems.Add(randomItem);
                sourceUnit.Battler.AddItemToInventory(randomItem); // プレイヤーのインベントリに追加
            }

            string resultItemMessage = $"{sourceUnit.Battler.Base.Name} obtained ";

            // 獲得したアイテムを表示
            foreach (Item item in awardedItems)
            {
                resultItemMessage += $"{item.Base.Name},";
            }
            sourceUnit.Battler.Money += targetUnit.Battler.Money;
            sourceUnit.Battler.Disk += targetUnit.Battler.Disk;
            if (playerUnit.Battler is PlayerBattler playerBattler)
            {
                playerBattler.UpdatePropertyPanel();  // PlayerBattler のメソッドを呼び出す
            }

            yield return StartCoroutine(messagePanel.TypeDialog(resultItemMessage));
            StartCoroutine(messagePanel.TypeDialog($"{playerUnit.Battler.Base.Name} won the battle.."));
        }
        else
        {
            yield return StartCoroutine(messagePanel.TypeDialog("No items were found on the enemy."));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(SetBattleState(BattleState.BattleOver));
    }

    public IEnumerator EnemyAttack()
    {
        Debug.Log("EnemyAttack");
        yield return StartCoroutine(AttackAction(enemyUnit, playerUnit));
        turnOrderSystem.EndTurn();
    }

    public void BattleEnd()
    {
        enemyUnit.gameObject.SetActive(false);
        playerUnit.SetMotion(MotionType.Move);
        actionController.CloseAction();
        OnBattleEnd?.Invoke();
    }
}
