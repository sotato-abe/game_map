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
    private readonly List<ActionType> actionList = new() { ActionType.Talk, ActionType.Attack, ActionType.Command, ActionType.Pouch, ActionType.Escape };
    private readonly List<ActionIcon> actionIconList = new();


    void Start()
    {
        transform.gameObject.SetActive(true);
        enemyUnit.gameObject.SetActive(false);

        actionBoard.OnExecuteBattleAction += ExecuteBattleAction;
        actionBoard.OnExitBattleAction += () => state = BattleState.ActionSelection;
        attackSystem.OnBattleResult += BattleResult;
        attackSystem.OnExecuteBattleAction += ExecuteBattleAction;
        attackSystem.OnBattleDefeat += BattleDefeat;
    }

    private void SetActionList()
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

    public void Update()
    {
        if (state == BattleState.ActionSelection || state == BattleState.TurnWait)
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = BattleState.ActionSelected;
            }
        }
    }

    private void SelectAction(ActionType selectAction)
    {
        actionBoard.ChangeActionPanel(selectAction);
        SelectActiveActionIcon(selectAction);
        activeAction = selectAction;
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = BattleState.TurnWait;
        SetActionList();
        StartCoroutine(SetupBattlers(player, enemy));
    }

    public IEnumerator SetupBattlers(Battler player, Battler enemy)
    {
        enemyUnit.Setup(enemy);
        enemyUnit.gameObject.SetActive(true);
        enemyUnit.SetMotion(MotionType.Jump);
        enemyUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。
        playerUnit.SetMessage(MessageType.Encount); // TODO : キャラクターメッセージリストから取得する。

        attackSystem.SetBattler(playerUnit, enemyUnit);
        turnOrderSystem.SetUpBattlerTurns(new List<Battler> { player, enemy });
        actionBoard.gameObject.SetActive(true);
        actionBoard.SetEventType(EventType.Battle);

        yield return messagePanel.TypeDialog($"{enemy.Base.Name} is coming!!");
    }

    public void StartActionSelection()
    {
        actionBoard.ChangeExecuteFlg(true);
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
        actionBoard.ChangeExecuteFlg(false);
        turnOrderSystem.EndTurn();
    }

    public void ExitBattleAction()
    {
        state = BattleState.ActionSelection;
    }

    public IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(0.5f);
        attackSystem.ExecuteEnemyAttack();
    }

    public void BattleResult()
    {
        actionBoard.ChangeExecuteFlg(false);
        List<string> resultItemMessageList = new List<string>();
        resultItemMessageList.Add($"{playerUnit.Battler.Base.Name} obtained ");
        List<Item> targetItems = enemyUnit.Battler.Inventory;

        if (targetItems != null && targetItems.Count > 0)
        {
            // TODO : アイテムからそれぞれのレア度を考慮して確率で取得

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
                bool success = playerUnit.Battler.AddItemToPouch(shuffledItems[i]); // プレイヤーのインベントリに追加
                if (!success)
                    playerUnit.Battler.AddItemToInventory(shuffledItems[i]); // プレイヤーのインベントリに追加
            }

            string itemList = "";

            // 獲得したアイテムを表示
            foreach (Item item in awardedItems)
            {
                itemList += $"{item.Base.Name},";
            }
            resultItemMessageList.Add($"{playerUnit.Battler.Base.Name} got {itemList}");

            playerUnit.Battler.Money += enemyUnit.Battler.Money;
            playerUnit.Battler.Disk += enemyUnit.Battler.Disk;
            if (playerUnit.Battler is PlayerBattler playerBattler)
            {
                playerBattler.UpdatePropertyPanel();  // PlayerBattler のメソッドを呼び出す
            }
        }
        else
        {
            resultItemMessageList.Add("No items were found on the enemy.");
        }

        StartCoroutine(BattleResultView(resultItemMessageList));
    }

    private IEnumerator BattleResultView(List<string> resultItemMessageList)
    {
        foreach (string message in resultItemMessageList)
        {
            yield return StartCoroutine(messagePanel.TypeDialog(message));
        }
        yield return new WaitForSeconds(1.5f);
        BattleEnd();
    }

    public void BattleEnd()
    {
        actionBoard.ChangeExecuteFlg(false);
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

    public void BattleDefeat()
    {
        Debug.Log("GameOver");
    }
}
