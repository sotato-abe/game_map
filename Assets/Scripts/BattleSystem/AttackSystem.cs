using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class AttackSystem : MonoBehaviour
{
    public UnityAction OnExecuteBattleAction;
    public UnityAction OnBattleDefeat;
    public UnityAction OnBattleEscape;
    private PlayerUnit playerUnit;
    private List<BattleUnit> enemyUnits = new List<BattleUnit>();
    private List<BattleUnit> allyUnits = new List<BattleUnit>();

    [SerializeField] private AttackPanel attackPanel;
    [SerializeField] private EscapePanel escapePanel;
    [SerializeField] TurnOrderSystem turnOrderSystem;
    [SerializeField] FieldCharacterSystem fieldCharacterSystem;

    private bool activePlayerTurn = false;
    public bool ActivePlayerTurn => activePlayerTurn;

    public void SetActivePlayerTurn(bool isActive)
    {
        activePlayerTurn = isActive;
    }

    public void SetPlayerBattler(PlayerUnit playerUnit)
    {
        allyUnits.Clear();
        this.playerUnit = playerUnit;
        allyUnits.Add(playerUnit);
    }

    public void SetEnemyBattlers(List<BattleUnit> enemyUnits)
    {
        this.enemyUnits = enemyUnits;
    }

    public void ExecutePlayerTalk() // 現在未使用中
    {
        playerUnit.SetTalkMessage("hey");
        enemyUnits[0].SetTalkMessage("...  ");

        // TODO : Talkのアクション実装
        // 確率でクエスト開放する
        // クエストを受注するとバトルは終了する

        EndPlayerTurn(playerUnit);
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
        EndPlayerTurn(attakerUnit);
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
        }
    }

    public IEnumerator ExecuteEnemyAttack(Battler attacker) // EnemyUnitに移動できそう
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
        attacks.Add(enemyUnit.Battler.GetAttack());
        fieldCharacterSystem.SetCharacterMotion(attacker, AnimationType.Attack);
        ExecuteBattlerAttack(enemyUnit.Battler, attacks, false);
        yield return new WaitForSeconds(0.5f);
    }

    public void ExecutePlayerEscape(bool isSuccess)
    {
        if (isSuccess)
        {
            playerUnit.SetBattlerTalkMessage(MessageType.Escape);
            enemyUnits[0].SetBattlerTalkMessage(MessageType.Escape);
            OnBattleEscape?.Invoke();
        }
        else
        {
            enemyUnits[0].SetBattlerTalkMessage(MessageType.Win);
            EndPlayerTurn(playerUnit);
        }
    }

    private void EndPlayerTurn(BattleUnit battlerUnit)
    {
        battlerUnit.DecreaseEnchant();
        activePlayerTurn = false;
        OnExecuteBattleAction?.Invoke();
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
