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

    public void ExecutePlayerAttack(List<Attack> attacks)
    {
        if (0 < attacks.Count)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Attack);
            enemyUnit.TakeAttack(attacks);
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

    public void ExecutePlayerEscape()
    {
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
        List<Attack> attacks = new List<Attack>();

        foreach (Equipment equipment in enemyUnit.Battler.EquipmentList)
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
            enemyUnit.SetBattlerTalkMessage(MessageType.Attack);
            playerUnit.TakeAttack(attacks);
        }
        if (playerUnit.Battler.Life <= 0)
        {
            OnBattleDefeat?.Invoke();
        }
        else
        {
            enemyUnit.DecreaseEnchant();
            OnExecuteBattleAction?.Invoke();
        }
    }

    public bool CheckEnegy(Equipment equipment)
    {
        int life = Mathf.Max(0, enemyUnit.Battler.Life);
        int battery = Mathf.Max(0, enemyUnit.Battler.Battery);
        int soul = Mathf.Max(0, enemyUnit.Battler.Soul);

        return
            equipment.EquipmentBase.LifeCost.val <= life &&
            equipment.EquipmentBase.BatteryCost.val <= battery &&
            equipment.EquipmentBase.SoulCost.val <= soul;
    }

    public void UseEnegy(Equipment equipment)
    {
        enemyUnit.Battler.Life -= equipment.EquipmentBase.LifeCost.val;
        enemyUnit.Battler.Battery -= equipment.EquipmentBase.BatteryCost.val;
        enemyUnit.Battler.Soul -= equipment.EquipmentBase.SoulCost.val;
        enemyUnit.UpdateEnegyUI();
    }
}
