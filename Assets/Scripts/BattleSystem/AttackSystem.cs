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
    private List<BattleUnit> enemyUnits;

    [SerializeField] private AttackPanel attackPanel;
    [SerializeField] MessagePanel messagePanel;

    public void SetBattler(BattleUnit playerUnit, BattleUnit enemyUnit)
    {
        this.playerUnit = playerUnit;
        this.enemyUnit = enemyUnit;
    }

    public void SetPlayerBattler(BattleUnit playerUnit)
    {
        this.playerUnit = playerUnit;
    }

    public void SetEnemyBattlers(List<BattleUnit> enemyUnits)
    {
        this.enemyUnits = enemyUnits;
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
            enemyUnits[0].TakeAttack(attacks);
        }
        if (enemyUnits[0].Battler.Life <= 0)
        {
            GetReward(enemyUnits[0].Battler);
            enemyUnits[0].SetBattlerTalkMessage(MessageType.Lose);
            Destroy(enemyUnits[0].gameObject); // TODO : 最後のモーションをさせる
            Debug.Log("test1");
            enemyUnits.RemoveAt(0);
            Debug.Log("test2");
        }

        if (enemyUnits.Count == 0)
        {
            Debug.Log($"全滅させた");
            playerUnit.SetTalkMessage("かった!!");
            OnBattleResult?.Invoke();
        }
        else
        {
            EndPlayerTurn();
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
            playerUnit.TakeAttack(attacks);
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
