using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


// バトルエンカウント処理はこっちに入れる
// バトルエンカウントは時間とフィールドの危険度を元にランダムで決まる。

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;

    [SerializeField] TurnOrderSystem turnOrderSystem;
    [SerializeField] ActionBoard actionBoard;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] AttackSystem attackSystem;
    [SerializeField] ActionIcon actionIconPrefab;
    [SerializeField] GameObject actionListObject;

    public BattleState state;
    private ActionType activeAction = ActionType.Talk;
    private ActionIcon selectedAction;
    private List<ActionType> actionList = new List<ActionType>();
    private List<ActionIcon> actionIconList = new List<ActionIcon>();

    void Start()
    {
        actionList.Add(ActionType.Talk);
        actionList.Add(ActionType.Attack);
        actionList.Add(ActionType.Command);
        actionList.Add(ActionType.Pouch);
        actionList.Add(ActionType.Escape);

        transform.gameObject.SetActive(true);
        enemyUnit.gameObject.SetActive(false);

        actionBoard.OnExecuteBattleAction += ExecuteBattleAction;
        actionBoard.OnExitBattleAction += ExitBattleAction;
    }

    public void SetState(BattleState targetState)
    {
        state = targetState;
    }

    public void Update()
    {
        if (state == BattleState.ActionSelection)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index + 1) % actionList.Count; // 次のインデックスへ（リストの範囲を超えたら先頭へ）
                activeAction = actionList[index]; // 更新
                SelectAction(activeAction);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index - 1 + actionList.Count) % actionList.Count; // 前のインデックスへ（負の値を回避）
                activeAction = actionList[index]; // 更新
                SelectAction(activeAction);
            }

            // if (Input.GetKeyDown(KeyCode.Return))
            // {
            //     state = BattleState.ActionSelected;
            // }
        }
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = BattleState.TurnWait;
        StartCoroutine(SetupBattle(player, enemy));
        playerUnit.SetMotion(MotionType.Jump);
        playerUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。
        enemyUnit.gameObject.SetActive(true);
        enemyUnit.SetMotion(MotionType.Jump);
        enemyUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。
        List<Battler> battlers = new List<Battler> { player, enemy };
        turnOrderSystem.SetUpBattlerTurns(battlers);
        actionBoard.gameObject.SetActive(true);
        actionBoard.SetEventType(EventType.Battle);
        setActionList();
    }


    private void setActionList()
    {
        foreach (ActionType actionValue in actionList)
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionListObject.transform);
            actionIconList.Add(actionIcon);
            actionIcon.SetAction(actionValue);
            if (activeAction == actionValue)
            {
                actionBoard.ChangeActionPanel(actionValue);
            }
        }

        selectedAction = actionIconList.Count > 0 ? actionIconList[0] : null;

        if (selectedAction)
        {
            selectedAction.SetActive(true);
        }
    }

    private void SelectAction(ActionType selectAction)
    {
        actionBoard.ChangeActionPanel(selectAction);
        SelectActiveActionIcon(selectAction);
        activeAction = selectAction;
    }

    public void ExecuteBattleAction()
    {
        switch (activeAction)
        {
            case ActionType.Talk:
                Debug.Log("Talk を開く処理を実行");
                break;

            case ActionType.Attack:
                Debug.Log("Attack を開く処理を実行");
                break;

            case ActionType.Command:
                Debug.Log("Command を開く処理を実行");
                break;

            case ActionType.Escape:
                Debug.Log("Escape を開く処理を実行");
                BattleEnd();
                break;

            default:
                Debug.Log("未定義のアクションが選択されました");
                break;
        }

        // アクション実行後は、State を Standby に戻す
        state = BattleState.ActionSelection;
    }

    public void ExitBattleAction()
    {
        state = BattleState.ActionSelection;
    }

    private void SelectActiveActionIcon(ActionType target)
    {
        // 現在選択中のアクションを非アクティブにする
        if (selectedAction != null)
        {
            selectedAction.SetActive(false);
        }

        // 対応するアクションアイコンを探してアクティブにする
        foreach (ActionIcon icon in actionIconList)
        {
            if (icon.type == activeAction)
            {
                selectedAction = icon;
                selectedAction.SetActive(false);
            }

            if (icon.type == target)
            {
                selectedAction = icon;
                selectedAction.SetActive(true);
            }
        }
    }

    public IEnumerator SetupBattle(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        actionBoard.ChangeActionPanel(ActionType.Talk);
        yield return StartCoroutine(messagePanel.TypeDialog($"{enemy.Base.Name} is coming!!"));
    }

    public IEnumerator BattleResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        StartCoroutine(targetUnit.SetTalkMessage("You'll regret this!!")); // TODO : キャラクターメッセージリストから取得する。
        targetUnit.SetMotion(MotionType.Jump);
        yield return StartCoroutine(messagePanel.TypeDialog($"{targetUnit.Battler.Base.Name} walked away\n{sourceUnit.Battler.Base.Name} win"));

        List<Item> targetItems = targetUnit.Battler.Inventory;
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
        SetState(BattleState.BattleResult);
    }

    public IEnumerator EnemyAttack()
    {
        Debug.Log("EnemyAttack");
        // yield return StartCoroutine(AttackAction(enemyUnit, playerUnit));
        yield return new WaitForSeconds(2f);
        turnOrderSystem.EndTurn();
    }

    public void BattleEnd()
    {
        state = BattleState.Standby;
        activeAction = actionList[0];
        turnOrderSystem.BattlerEnd();
        foreach (ActionIcon icon in actionIconList)
        {
            Destroy(icon.gameObject);
        }
        actionIconList.Clear();
        actionBoard.ClosePanel();
        enemyUnit.gameObject.SetActive(false);
        playerUnit.SetMotion(MotionType.Move);
        OnBattleEnd?.Invoke();
    }
}
