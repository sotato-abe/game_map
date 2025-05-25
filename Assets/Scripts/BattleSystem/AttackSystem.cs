using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class AttackSystem : MonoBehaviour
{
    public UnityAction OnBattleResult;
    public UnityAction OnExecuteBattleAction;
    public UnityAction OnBattleDefeat;
    public UnityAction OnBattleEscape;
    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;
    private List<BattleUnit> enemyUnits = new List<BattleUnit>();
    private List<BattleUnit> allyUnits = new List<BattleUnit>();

    [SerializeField] private AttackPanel attackPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] TurnOrderSystem turnOrderSystem;

    public void SetBattler(BattleUnit playerUnit, BattleUnit enemyUnit)
    {
        this.playerUnit = playerUnit;
        this.enemyUnit = enemyUnit;
    }

    public void SetPlayerBattler(BattleUnit playerUnit)
    {
        allyUnits.Clear();
        this.playerUnit = playerUnit;
        allyUnits.Add(playerUnit);
    }

    public void SetEnemyBattlers(List<BattleUnit> enemyUnits)
    {
        this.enemyUnits = enemyUnits;
        SetEnemyListToPanel();
    }

    private void SetEnemyListToPanel()
    {
        List<Battler> enemyBattlers = new List<Battler>();
        foreach (BattleUnit enemyUnit in enemyUnits)
        {
            enemyBattlers.Add(enemyUnit.Battler);
        }
        escapePanel.SetEnemyList(enemyBattlers);
    }

    public void ExecutePlayerTalk()
    {
        playerUnit.SetTalkMessage("hey");
        enemyUnits[0].SetTalkMessage("...  ");

        // TODO : Talkのアクション実装
        // 確率でクエスト開放する
        // クエストを受注するとバトルは終了する

        EndPlayerTurn();
    }


    public void ExecuteBattlerAttack(Battler attaker, List<Attack> attacks, bool isAlly)
    {
        List<BattleUnit> targetUnits = isAlly ? allyUnits : enemyUnits;
        BattleUnit attakerUnit = targetUnits.FirstOrDefault(unit => unit.Battler == attaker);
        if (0 < attacks.Count)
        {
            attakerUnit.SetBattlerTalkMessage(MessageType.Attack);
            ExecuteAttack(attakerUnit, attacks, isAlly);
        }
        else
        {
            // TODO : 攻撃失敗の演出
            attakerUnit.SetBattlerTalkMessage(MessageType.Miss);
        }
        ConfirmationSurvival();
        SetEnemyListToPanel();
        EndPlayerTurn();
    }

    public void ExecuteEnemyAttack(Battler attacker)
    {
        List<Attack> attacks = new List<Attack>();

        BattleUnit enemyUnit = enemyUnits.FirstOrDefault(unit => unit.Battler == attacker);

        foreach (Equipment equipment in enemyUnit.Battler.EquipmentList)
        {
            if (CheckEnegy(equipment) == false)
            {
                continue;
            }

            if (Random.Range(0, 100) < equipment.EquipmentBase.Probability)
            {
                // エネジーを消費する
                enemyUnit.Battler.Life -= equipment.EquipmentBase.LifeCost.val;
                enemyUnit.Battler.Battery -= equipment.EquipmentBase.BatteryCost.val;
                enemyUnit.Battler.Soul -= equipment.EquipmentBase.SoulCost.val;
                enemyUnit.UpdateEnegyUI();
                attacks.Add(equipment.Attack);
            }
        }
        ExecuteBattlerAttack(enemyUnit.Battler, attacks, false);
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
        messagePanel.AddMessage(MessageIconType.Battle, resultItemMessageList);
    }

    public void ExecutePlayerEscape()
    {
        playerUnit.SetBattlerTalkMessage(MessageType.Escape);
        enemyUnits[0].SetTalkMessage("まて!!");
        OnBattleEscape?.Invoke();

    }

    private void EndPlayerTurn()
    {
        Debug.Log($"EndPlayerTurn");
        playerUnit.DecreaseEnchant();
        OnExecuteBattleAction?.Invoke();
    }

    private void ExecuteAttack(BattleUnit attackerUnit, List<Attack> attacks, bool isAllyAttack = true)
    {
        List<BattleUnit> allies = isAllyAttack ? allyUnits : enemyUnits;
        List<BattleUnit> enemies = isAllyAttack ? enemyUnits : allyUnits;

        foreach (Attack attack in attacks)
        {
            switch (attack.Target)
            {
                case TargetType.Own:
                    attackerUnit.TakeAttack(attack);
                    break;

                case TargetType.AllyFront:
                    if (allies.Count > 0)
                        allies[0].TakeAttack(attack);
                    break;

                case TargetType.AllyAll:
                    foreach (var unit in allies)
                        unit.TakeAttack(attack);
                    break;

                case TargetType.EnemyFront:
                    if (enemies.Count > 0)
                        enemies[0].TakeAttack(attack);
                    break;

                case TargetType.EnemyAll:
                    foreach (var unit in enemies)
                        unit.TakeAttack(attack);
                    break;

                default: // TargetType.All など
                    foreach (var unit in allies)
                        unit.TakeAttack(attack);
                    foreach (var unit in enemies)
                        unit.TakeAttack(attack);
                    break;
            }

            break; // 一回の攻撃で終了ならここで break
        }
    }

    private void ConfirmationSurvival()
    {
        if (playerUnit.Battler.Life <= 0)
        {
            Debug.Log("Lose"); // TODO : プレイヤー敗北の演出：シーン変更
            playerUnit.SetBattlerTalkMessage(MessageType.Lose);
            OnBattleDefeat?.Invoke();
        }
        else
        {
            for (int i = enemyUnits.Count - 1; i >= 0; i--)
            {
                BattleUnit enemyUnit = enemyUnits[i];
                if (enemyUnit.Battler.Life <= 0)
                {
                    GetReward(enemyUnit.Battler);
                    enemyUnit.SetBattlerTalkMessage(MessageType.Lose);
                    turnOrderSystem.RemoveTurnBattler(enemyUnit.Battler);
                    StartCoroutine(OutOfLineBattler(enemyUnit));
                }
            }
            for (int i = allyUnits.Count - 1; i >= 0; i--)
            {
                BattleUnit allyUnit = allyUnits[i];
                if (allyUnit.Battler.Life <= 0)
                {
                    allyUnit.SetBattlerTalkMessage(MessageType.Lose);
                    turnOrderSystem.RemoveTurnBattler(allyUnit.Battler);
                    StartCoroutine(OutOfLineBattler(allyUnit));
                }
            }
        }
    }


    private IEnumerator OutOfLineBattler(BattleUnit battlerUnit)
    {
        Debug.Log("test1");
        battlerUnit.SetMotion(MotionType.Rotate);
        if (enemyUnits.Contains(battlerUnit))
        {
            enemyUnits.Remove(battlerUnit);
            Destroy(battlerUnit.gameObject); // TODO : 最後のモーションをさせる
        }
        else if (allyUnits.Contains(battlerUnit))
        {
            allyUnits.Remove(battlerUnit);
            Destroy(battlerUnit.gameObject); // TODO : 最後のモーションをさせる
        }
        yield return new WaitForSeconds(0.5f);
        if (enemyUnits.Count == 0)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Win);
            OnBattleResult?.Invoke();
        }
    }

    public bool CheckEnegy(Equipment equipment)
    {
        int life = Mathf.Max(0, enemyUnits[0].Battler.Life);
        int battery = Mathf.Max(0, enemyUnits[0].Battler.Battery);
        int soul = Mathf.Max(0, enemyUnits[0].Battler.Soul);

        return
            equipment.EquipmentBase.LifeCost.val <= life &&
            equipment.EquipmentBase.BatteryCost.val <= battery &&
            equipment.EquipmentBase.SoulCost.val <= soul;
    }
}
