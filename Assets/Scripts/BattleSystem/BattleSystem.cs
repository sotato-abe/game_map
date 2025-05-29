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
    [SerializeField] FieldCharacterSystem fieldCharacterSystem;
    [SerializeField] BattleActionBoard actionBoard;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] PlayerUnit playerUnit;
    [SerializeField] BattleUnit allyUnitPrefab;
    [SerializeField] BattleUnit enemyUnitPrefab;
    [SerializeField] AttackSystem attackSystem;
    [SerializeField] GameObject leftGroupPanel;
    [SerializeField] GameObject rightGroupPanel;

    private List<BattleUnit> allyUnitList = new List<BattleUnit>();
    private List<BattleUnit> enemyUnitList = new List<BattleUnit>();

    void Start()
    {
        transform.gameObject.SetActive(true);
        attackSystem.OnExecuteBattleAction += ExecuteBattleAction;
        attackSystem.OnBattleEscape += BattleEscape;
        attackSystem.OnBattleDefeat += BattleDefeat;
    }

    public void SetBattle(List<Battler> enemies)
    {
        turnOrderSystem.TurnOrderClear();
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
        attackSystem.SetPlayerBattler(playerUnit);
        attackSystem.SetEnemyBattlers(enemyUnitList);
        turnOrderSystem.SetActive(true);
        actionBoard.gameObject.SetActive(true);
        actionBoard.SetEnemyListToPanel(enemies);
    }

    public void SetBattlerUnit(Battler battler, bool isAlly)
    {
        if (isAlly)
        {
            BattleUnit battlerUnit = Instantiate(allyUnitPrefab, leftGroupPanel.transform);
            battlerUnit.Setup(battler);
            battlerUnit.SetFieldCharacterSystem(fieldCharacterSystem);
            battlerUnit.SetMotion(MotionType.Jump);
            battlerUnit.SetBattlerTalkMessage(MessageType.Encount);
            allyUnitList.Add(battlerUnit);
        }
        else
        {
            BattleUnit battlerUnit = Instantiate(enemyUnitPrefab, rightGroupPanel.transform);
            battlerUnit.Setup(battler);
            battlerUnit.SetFieldCharacterSystem(fieldCharacterSystem);
            battlerUnit.SetMotion(MotionType.Jump);
            battlerUnit.SetBattlerTalkMessage(MessageType.Encount);
            enemyUnitList.Add(battlerUnit);
        }
    }

    public void ExecuteBattleAction()
    {
        ConfirmationSurvival();
        ReSetEnemyList();
        turnOrderSystem.EndTurn();
    }

    public IEnumerator EnemyAttack(Battler attaker)
    {
        yield return new WaitForSeconds(0.5f);
        attackSystem.ExecuteEnemyAttack(attaker);
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

    public void BattleEnd()
    {
        turnOrderSystem.BattlerEnd();
        playerUnit.SetMotion(MotionType.Move);
        enemyUnitList.Clear();
        actionBoard.gameObject.SetActive(false);
        fieldCharacterSystem.RemoveAllCharacter(); // 敵を削除
        OnBattleEnd?.Invoke();
    }

    public void BattleDefeat()
    {
        Debug.Log("ゲームオーバー");
        fieldCharacterSystem.SetCharacterMotion(playerUnit.Battler, AnimationType.Death);
        // ゲームオーバー処理をここに追加
    }


    private void ConfirmationSurvival()
    {
        if (playerUnit.Battler.Life <= 0)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Lose);
            messagePanel.AddMessage(MessageIconType.System, "ゲームオーバー...");
            BattleDefeat();
        }
        else
        {
            for (int i = enemyUnitList.Count - 1; i >= 0; i--)
            {
                BattleUnit enemyUnit = enemyUnitList[i];
                if (enemyUnit.Battler.Life <= 0)
                {
                    GetReward(enemyUnit.Battler);
                    StartCoroutine(OutOfLineBattler(enemyUnit));
                }
            }
            for (int i = allyUnitList.Count - 1; i >= 0; i--)
            {
                BattleUnit allyUnit = allyUnitList[i];
                if (allyUnit.Battler.Life <= 0)
                {
                    StartCoroutine(OutOfLineBattler(allyUnit));
                }
            }
        }
    }

    private void ReSetEnemyList()
    {
        List<Battler> enemyBattlers = new List<Battler>();
        foreach (BattleUnit enemyUnit in enemyUnitList)
        {
            enemyBattlers.Add(enemyUnit.Battler);
        }
        actionBoard.SetEnemyListToPanel(enemyBattlers);
    }

    private IEnumerator OutOfLineBattler(BattleUnit battlerUnit)
    {
        battlerUnit.SetBattlerTalkMessage(MessageType.Lose);
        battlerUnit.SetMotion(MotionType.Rotate);
        turnOrderSystem.RemoveTurnBattler(battlerUnit.Battler);
        if (enemyUnitList.Contains(battlerUnit))
        {
            enemyUnitList.Remove(battlerUnit);
        }
        else if (allyUnitList.Contains(battlerUnit))
        {
            allyUnitList.Remove(battlerUnit);
        }
        yield return new WaitForSeconds(1.0f); // モーションの時間を待つ
        Destroy(battlerUnit.gameObject);
        fieldCharacterSystem.RemoveFieldCharacter(battlerUnit.Battler); // フィールドからキャラクターを削除

        // 勝利条件の確認
        if (enemyUnitList.Count == 0)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Win);
            BattleEnd();
        }
    }

    private void GetReward(Battler battler)
    {
        List<Consumable> targetItems = battler.PouchList;
        string resultItemMessageList = "";
        resultItemMessageList = battler.Base.Name + " に勝利した。\n";

        if (targetItems != null && targetItems.Count > 0)
        {
            string itemList = "";
            List<Consumable> awardedItems = new List<Consumable>();

            foreach (Consumable item in targetItems)
            {
                // TODO：アイテムのレア度によって取得確率を変える
                if (Random.Range(0, 100) < item.Base.Rarity.GetProbability())
                {
                    bool success = playerUnit.Battler.AddItem(item); // プレイヤーのインベントリに追加
                    if (success)
                    {
                        itemList += $"{item.Base.Name},";
                    }
                }
            }

            if (itemList != "")
            {
                resultItemMessageList += ($"{itemList}を手に入れた。\n");
            }
        }
        else
        {
            resultItemMessageList += ($"{battler.Base.Name} は何も持っていなかった。\n");
        }

        if (playerUnit.Battler is PlayerBattler playerBattler)
        {
            string prizeText = "";
            if (battler.Money > 0)
            {
                prizeText += ($"ゼニ：{battler.Money} Z、");
                playerUnit.Battler.Money += battler.Money;
            }
            if (battler.Disk > 0)
            {
                prizeText += ($"ディスク：{battler.Disk}、");
                playerUnit.Battler.Disk += battler.Disk;
            }
            if (prizeText != "")
            {
                resultItemMessageList += ($"{prizeText}を手に入れた。\n");
                playerBattler.UpdatePropertyPanel();  // PlayerBattler のメソッドを呼び出す
            }
            playerBattler.AcquisitionExp(battler.Exp); // プレイヤーの経験値を加算
            resultItemMessageList += ($"経験値を{battler.Exp}手に入れた。");
        }
        playerUnit.CheckSkillPoint();
        messagePanel.AddMessage(MessageIconType.Battle, resultItemMessageList);
    }
}
