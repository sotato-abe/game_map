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
    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;

    [SerializeField] private AttackPanel attackPanel;

    public void SetBattler(BattleUnit playerUnit, BattleUnit enemyUnit)
    {
        this.playerUnit = playerUnit;
        this.enemyUnit = enemyUnit;
    }

    public void ExecutePlayerAttack(List<Damage> damages)
    {
        if (0 < damages.Count)
        {
            enemyUnit.Battler.TakeDamage(damages);
            enemyUnit.UpdateEnegyUI();
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
