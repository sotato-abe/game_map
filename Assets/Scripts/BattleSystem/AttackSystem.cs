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

    public void ExecutePlayerAttack(List<Attack> attacks)
    {
        if (0 < attacks.Count)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Attack);
            // TODO : Battlerのステータスを参照して、攻撃の値を変更する     
            // TODO : attacksをtarget毎に分ける
            foreach (Attack attack in attacks)
            {
                if (attack.Target == TargetType.Enemy)
                {
                    Debug.Log("Enemy");
                    enemyUnits[0].TakeAttack(attack);
                }
                else if (attack.Target == TargetType.EnemyAll)
                {
                    Debug.Log("EnemyAll");
                    // TODO : 右全体にアタックする場合の処理
                    foreach (BattleUnit enemyUnit in enemyUnits)
                    {
                        enemyUnit.TakeAttack(attack);
                    }
                }
                else if (attack.Target == TargetType.Own)
                {
                    Debug.Log("Own");
                    // TODO : 自身にアタックする場合の処理
                    playerUnit.TakeAttack(attack);
                }
                else if (attack.Target == TargetType.Ally)
                {
                    Debug.Log("Ally");
                    // TODO : 左全体にアタックする場合の処理
                    foreach (BattleUnit allyUnit in allyUnits)
                    {
                        allyUnit.TakeAttack(attack);
                    }
                }
                else
                {
                    Debug.Log("All");
                    // TODO : 全体にアタックする場合の処理
                    foreach (BattleUnit enemyUnit in enemyUnits)
                    {
                        enemyUnit.TakeAttack(attack);
                    }
                    foreach (BattleUnit allyUnit in allyUnits)
                    {
                        allyUnit.TakeAttack(attack);
                    }
                }
            }
        }
        for (int i = enemyUnits.Count - 1; i >= 0; i--)
        {
            BattleUnit enemyUnit = enemyUnits[i];
            if (enemyUnit.Battler.Life <= 0)
            {
                GetReward(enemyUnit.Battler);
                enemyUnit.SetBattlerTalkMessage(MessageType.Lose);
                turnOrderSystem.RemoveTurnBattler(enemyUnit.Battler);
                enemyUnits.RemoveAt(i);
                Destroy(enemyUnit.gameObject); // TODO : 最後のモーションをさせる
            }
        }
        if (enemyUnits.Count == 0)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Win);
            OnBattleResult?.Invoke();
        }
        SetEnemyListToPanel();
        EndPlayerTurn();
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
        }

        if (playerUnit.Battler is PlayerBattler playerBattler)
        {
            playerBattler.AcquisitionExp(battler.Exp); // プレイヤーの経験値を加算
            resultItemMessageList += ($"経験値を{battler.Exp}手に入れた。");
            playerBattler.UpdatePropertyPanel();  // PlayerBattler のメソッドを呼び出す
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

    public void ExecuteEnemyAttack()
    {
        List<Attack> attacks = new List<Attack>();

        foreach (Equipment equipment in enemyUnits[0].Battler.EquipmentList)
        {
            if (CheckEnegy(equipment) == false)
            {
                continue;
            }

            if (Random.Range(0, 100) < equipment.EquipmentBase.Probability)
            {
                // エネジーを消費する
                UseEnegy(equipment);
                attacks.Add(equipment.Attack);
            }
        }

        if (0 < attacks.Count)
        {
            enemyUnits[0].SetBattlerTalkMessage(MessageType.Attack);
            foreach (Attack attack in attacks)
            {
                if (attack.Target == TargetType.Enemy)
                {
                    Debug.Log("Enemy");
                    playerUnit.TakeAttack(attack);
                }
                else if (attack.Target == TargetType.EnemyAll)
                {
                    Debug.Log("EnemyAll");
                    // TODO : 右全体にアタックする場合の処理
                    foreach (BattleUnit batleUnit in allyUnits)
                    {
                        batleUnit.TakeAttack(attack);
                    }
                }
                else if (attack.Target == TargetType.Own)
                {
                    Debug.Log("Own");
                    // TODO : 自身にアタックする場合の処理
                    enemyUnits[0].TakeAttack(attack);
                }
                else if (attack.Target == TargetType.Ally)
                {
                    Debug.Log("Ally");
                    // TODO : 左全体にアタックする場合の処理
                    foreach (BattleUnit battleUnit in enemyUnits)
                    {
                        battleUnit.TakeAttack(attack);
                    }
                }
                else
                {
                    Debug.Log("All");
                    // TODO : 全体にアタックする場合の処理
                    foreach (BattleUnit enemyUnit in enemyUnits)
                    {
                        enemyUnit.TakeAttack(attack);
                    }
                    foreach (BattleUnit allyUnit in allyUnits)
                    {
                        allyUnit.TakeAttack(attack);
                    }
                }
            }
        }
        if (playerUnit.Battler.Life <= 0)
        {
            OnBattleDefeat?.Invoke();
        }
        else
        {
            enemyUnits[0].DecreaseEnchant();
            OnExecuteBattleAction?.Invoke();
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

    public void UseEnegy(Equipment equipment)
    {
        enemyUnits[0].Battler.Life -= equipment.EquipmentBase.LifeCost.val;
        enemyUnits[0].Battler.Battery -= equipment.EquipmentBase.BatteryCost.val;
        enemyUnits[0].Battler.Soul -= equipment.EquipmentBase.SoulCost.val;
        enemyUnits[0].UpdateEnegyUI();
    }
}
