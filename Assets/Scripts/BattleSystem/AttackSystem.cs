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
        StartCoroutine(playerUnit.SetTalkMessage("hey"));
        StartCoroutine(enemyUnit.SetTalkMessage("...  "));

        // TODO : Talkのアクション実装
        // 確率でクエスト開放する
        // クエストを受注するとバトルは終了する

        OnExecuteBattleAction?.Invoke();
    }

    public void ExecutePlayerAttack(List<Damage> damages)
    {
        if (0 < damages.Count)
        {
            enemyUnit.Battler.TakeDamage(damages);
            enemyUnit.UpdateEnegyUI();
            StartCoroutine(enemyUnit.SetTalkMessage("Auch!!"));
            enemyUnit.SetMotion(MotionType.Shake);
        }
        if (enemyUnit.Battler.Life <= 0)
        {
            OnBattleResult?.Invoke();
        }
        else
        {
            OnExecuteBattleAction?.Invoke();
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
                playerUnit.Battler.TakeEnchant(playerEnchants);
                playerUnit.UpdateEnegyUI();
                playerUnit.SetMotion(MotionType.Shake);
            }
            if (enemyEnchants.Count > 0)
            {
                enemyUnit.Battler.TakeEnchant(enemyEnchants);
                enemyUnit.UpdateEnegyUI();
                enemyUnit.SetMotion(MotionType.Shake);
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
            OnExecuteBattleAction?.Invoke();
        }
    }

    public void ExecutePlayerEscape()
    {
        // TODO : 逃亡時の処理を追加
        StartCoroutine(playerUnit.SetTalkMessage("Run!"));
        StartCoroutine(enemyUnit.SetTalkMessage("wait!!"));
        OnBattleEscape?.Invoke();
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
                    Damage damage = new Damage(AttackType.Enegy, (int)attack.type, attack.val);
                    damages.Add(damage);
                }
            }
        }

        if (0 < damages.Count)
        {
            playerUnit.SetMotion(MotionType.Shake);
            playerUnit.Battler.TakeDamage(damages);
            StartCoroutine(playerUnit.SetTalkMessage("Dumm,"));
            playerUnit.UpdateEnegyUI();
        }
        if (playerUnit.Battler.Life <= 0)
        {
            OnBattleDefeat?.Invoke();
        }
        else
        {
            OnExecuteBattleAction?.Invoke();
        }
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
