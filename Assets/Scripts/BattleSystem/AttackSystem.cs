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

    [SerializeField] private AttackPanel attackPanel;

    public void SetBattler(BattleUnit playerUnit, BattleUnit enemyUnit)
    {
        this.playerUnit = playerUnit;
        this.enemyUnit = enemyUnit;
    }

    public void ExecutePlayerTalk()
    {
        playerUnit.SetTalkMessage("hey");
        enemyUnit.SetTalkMessage("...  ");

        // TODO : Talkのアクション実装
        // 確率でクエスト開放する
        // クエストを受注するとバトルは終了する

        EndPlayerTurn();
    }

    public void ExecutePlayerAttack(List<Damage> damages)
    {
        if (0 < damages.Count)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Attack);
            enemyUnit.TakeDamage(damages);
        }
        if (enemyUnit.Battler.Life <= 0)
        {
            enemyUnit.SetBattlerTalkMessage(MessageType.Lose);
            OnBattleResult?.Invoke();
        }
        else
        {
            EndPlayerTurn();
        }
    }

    public void ExecutePlayerCommand(List<EnchantCount> enchantCounts)
    {
        if (0 < enchantCounts.Count)
        {
            List<Enchant> playerEnchants = new List<Enchant>();
            List<Enchant> enemyEnchants = new List<Enchant>();

            foreach (EnchantCount enchantCount in enchantCounts)
            {
                // 自身へのエンチャント
                if (enchantCount.Target == TargetType.Own || enchantCount.Target == TargetType.Ally || enchantCount.Target == TargetType.All)
                {
                    Enchant enchant = new Enchant(enchantCount.Type, enchantCount.Val);
                    playerEnchants.Add(enchant);

                }

                // 相手へのエンチャント
                if (enchantCount.Target == TargetType.Opponent || enchantCount.Target == TargetType.Enemy || enchantCount.Target == TargetType.All)
                {
                    Enchant enchant = new Enchant(enchantCount.Type, enchantCount.Val);
                    enemyEnchants.Add(enchant);
                }
            }
            if (playerEnchants.Count > 0)
            {
                playerUnit.TakeEnchant(playerEnchants);
            }
            if (enemyEnchants.Count > 0)
            {
                enemyUnit.TakeEnchant(enemyEnchants);
            }
        }
        if (playerUnit.Battler.Life <= 0)
        {
            OnBattleDefeat?.Invoke();
        }
        if (enemyUnit.Battler.Life <= 0)
        {
            OnBattleResult?.Invoke();
        }
        else
        {
            EndPlayerTurn();
        }
    }

    public void ExecutePlayerEscape()
    {
        // TODO : 逃亡時の処理を追加
        // playerUnit.SetTalkMessage("Run!");
        playerUnit.SetBattlerTalkMessage(MessageType.Escape);
        enemyUnit.SetTalkMessage("まて!!");
        OnBattleEscape?.Invoke();

    }
    
    private void EndPlayerTurn()
    {
        playerUnit.DecreaseEnchant();
        OnExecuteBattleAction?.Invoke();
    }

    public void ExecuteEnemyAttack()
    {
        List<Damage> damages = new List<Damage>();

        foreach (Equipment equipment in enemyUnit.Battler.Equipments)
        {
            if (CheckEnegy(equipment) == false)
            {
                continue;
            }

            if (Random.Range(0, 100) < equipment.Base.Probability)
            {
                UseEnegy(equipment);
                foreach (var attack in equipment.Base.AttackList)
                {
                    Damage damage = new Damage(AttackType.Enegy, attack.type, attack.val);
                    damages.Add(damage);
                }
            }
        }

        if (0 < damages.Count)
        {
            enemyUnit.SetBattlerTalkMessage(MessageType.Attack);
            playerUnit.TakeDamage(damages);
        }
        if (playerUnit.Battler.Life <= 0)
        {
            OnBattleDefeat?.Invoke();
        }
        else
        {
            EndEnemyTurn();
        }
    }

    private void EndEnemyTurn()
    {
        enemyUnit.DecreaseEnchant();
        OnExecuteBattleAction?.Invoke();
    }

    public bool CheckEnegy(Equipment equipment)
    {
        if (
            equipment.Base.LifeCost.val <= enemyUnit.Battler.Life &&
            equipment.Base.BatteryCost.val <= enemyUnit.Battler.Battery &&
            equipment.Base.SoulCost.val <= enemyUnit.Battler.Soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(Equipment equipment)
    {
        enemyUnit.Battler.Life -= equipment.Base.LifeCost.val;
        enemyUnit.Battler.Battery -= equipment.Base.BatteryCost.val;
        enemyUnit.Battler.Soul -= equipment.Base.SoulCost.val;
        enemyUnit.UpdateEnegyUI();
    }
}
