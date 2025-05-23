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
    [SerializeField] BattleUnit allyUnitPrefab;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleUnit enemyUnitPrefab;
    [SerializeField] AttackSystem attackSystem;
    [SerializeField] ActionIcon actionIconPrefab;
    [SerializeField] GameObject actionListObject;
    [SerializeField] GameObject leftGroupPanel;
    [SerializeField] GameObject rightGroupPanel;

    public BattleState state;
    private ActionType activeAction = ActionType.Attack;
    private ActionIcon selectedAction;
    private readonly List<ActionType> actionList = new List<ActionType> { };
    private readonly List<ActionIcon> actionIconList = new();

    private List<BattleUnit> allyUnitList = new List<BattleUnit>();
    private List<BattleUnit> enemyUnitList = new List<BattleUnit>();


    void Start()
    {
        // actionList.Add(ActionType.Talk);
        actionList.Add(ActionType.Attack);
        actionList.Add(ActionType.Command);
        actionList.Add(ActionType.Pouch);
        actionList.Add(ActionType.Escape);

        transform.gameObject.SetActive(true);
        enemyUnit.gameObject.SetActive(false);

        actionBoard.OnExecuteBattleAction += ExecuteBattleAction;
        actionBoard.OnExitBattleAction += () => state = BattleState.ActionSelection;
        attackSystem.OnBattleResult += BattleResult;
        attackSystem.OnExecuteBattleAction += ExecuteBattleAction;
        attackSystem.OnBattleEscape += BattleEscape;
        attackSystem.OnBattleDefeat += BattleDefeat;
    }

    private void SetActionList()
    {
        foreach (ActionType actionValue in actionList)
        {
            ActionIcon actionIcon = Instantiate(actionIconPrefab, actionListObject.transform);
            actionIcon.OnPointerEnterAction += SelectAction;
            actionIcon.SetAction(actionValue);
            actionIconList.Add(actionIcon);
            if (activeAction == actionValue)
            {
                actionBoard.ChangeActionPanel(actionValue);
            }
        }
        SelectActiveActionIcon(activeAction);
        actionBoard.ChangeActionPanel(activeAction);
    }

    public void Update()
    {
        if (state == BattleState.ActionSelection || state == BattleState.TurnWait)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index + 1) % actionList.Count; // 次のインデックスへ（リストの範囲を超えたら先頭へ）
                ActionType selectAction = actionList[index]; // 更新
                SelectAction(selectAction);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int index = actionList.IndexOf(activeAction); // 現在のactiveActionのインデックスを取得
                index = (index - 1 + actionList.Count) % actionList.Count; // 前のインデックスへ（負の値を回避）
                ActionType selectAction = actionList[index]; // 更新
                SelectAction(selectAction);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = BattleState.ActionSelected;
            }
        }
    }

    private void SelectAction(ActionType selectAction)
    {
        if (state == BattleState.ActionSelection || state == BattleState.TurnWait)
        {
            if (activeAction == selectAction) return;
            activeAction = selectAction;
            SelectActiveActionIcon(selectAction);
            actionBoard.ChangeActionPanel(selectAction);
        }
    }

    public void SetBattle(List<Battler> enemies)
    {
        state = BattleState.TurnWait;
        SetActionList();
        turnOrderSystem.ReSetTurnOrderSystem();
        turnOrderSystem.SetupPlayerBattler(playerUnit.Battler);
        playerUnit.SetBattlerTalkMessage(MessageType.Encount);
        foreach (Transform child in rightGroupPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Battler enemy in enemies)
        {
            turnOrderSystem.SetTurnBattler(enemy);
            SetBattlerUnit(enemy, false);
        }
        // attackSystem.SetBattler(playerUnit, enemyUnit);
        attackSystem.SetPlayerBattler(playerUnit);
        attackSystem.SetEnemyBattlers(enemyUnitList);
        turnOrderSystem.SetActive(true);
        actionBoard.gameObject.SetActive(true);
        actionBoard.SetEventType(EventType.Battle);
    }

    public void SetBattlerUnit(Battler battler, bool isAlly)
    {
        if (isAlly)
        {
            BattleUnit battlerUnit = Instantiate(allyUnitPrefab, leftGroupPanel.transform);
            battlerUnit.Setup(battler);
            enemyUnit.SetMotion(MotionType.Jump);
            battlerUnit.SetBattlerTalkMessage(MessageType.Encount);
            allyUnitList.Add(battlerUnit);
        }
        else
        {
            BattleUnit enemyUnit = Instantiate(enemyUnitPrefab, rightGroupPanel.transform);
            enemyUnit.Setup(battler);
            enemyUnit.SetMotion(MotionType.Jump);
            enemyUnit.SetBattlerTalkMessage(MessageType.Encount);
            enemyUnitList.Add(enemyUnit);
        }
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
                Debug.Log("Talk 処理を実行");
                break;

            case ActionType.Attack:
                Debug.Log("Attack 処理を実行");
                break;

            case ActionType.Command:
                Debug.Log("Command 処理を実行");
                break;

            case ActionType.Escape:
                Debug.Log("Escape 処理を実行");
                break;

            default:
                Debug.LogWarning("未定義のアクションが選択されました");
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

    private void BattleEscape()
    {
        StartCoroutine(EscapeResultView());
    }

    private IEnumerator EscapeResultView()
    {
        yield return new WaitForSeconds(1.5f);
        BattleEnd();
    }

    // TODO : リワード移動したしこの処理をスキップしてもいいかも。
    public void BattleResult()
    {
        actionBoard.ChangeExecuteFlg(false);
        BattleEnd();
    }

    public void BattleEnd()
    {
        actionBoard.ChangeExecuteFlg(false);
        state = BattleState.Standby;
        turnOrderSystem.BattlerEnd();
        foreach (ActionIcon icon in actionIconList)
        {
            Destroy(icon.gameObject);
        }
        actionIconList.Clear();
        actionBoard.ClosePanel();
        playerUnit.SetMotion(MotionType.Move);
        enemyUnitList.Clear();
        OnBattleEnd?.Invoke();
    }

    public void BattleDefeat()
    {
        playerUnit.SetBattlerTalkMessage(MessageType.Lose);
        messagePanel.AddMessage(MessageIconType.System, "ゲームオーバー...");
    }
}
