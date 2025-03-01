using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackSystem : MonoBehaviour
{
    private BattleUnit sourceUnit;
    private BattleUnit targetUnit;

    [SerializeField] private AttackPanel attackPanel;

    public void ExecuteAttack(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        this.sourceUnit = sourceUnit;
        this.targetUnit = targetUnit;

        List<Damage> damageList = CalculateDamage();
        if (0 < damageList.Count)
        {
            TakeDamage(damageList);
            this.targetUnit.UpdateEnegyUI();
            this.targetUnit.SetMotion(MotionType.Shake);
        }
        AttackResult();
    }

    // ダメージを計算
    private List<Damage> CalculateDamage()
    {
        // TODO : ちゃんとPlayer判定をする
        if (sourceUnit.Battler.Base.Name == "Sola")
        {
            return CalculateDamageSola();
        }
        else
        {
            return CalculateDamageEnemy();

        }
    }

    private List<Damage> CalculateDamageSola()
    {
        attackPanel.gameObject.SetActive(true);
        return attackPanel.ActivateEquipments();
    }

    private List<Damage> CalculateDamageEnemy()
    {
        List<Damage> damageList = new List<Damage>();

        sourceUnit.Battler.Equipments.ForEach(equipment =>
        {
            if (Random.Range(0, 100) < equipment.Base.Probability)
            {
                equipment.Base.AttackList.ForEach(attack =>
                {
                    Damage damage = new Damage(AttackType.Enegy, (int)attack.type, attack.val);
                    damageList.Add(damage);
                });
            }
        });

        return damageList;
    }

    // ダメージを適用
    private void TakeDamage(List<Damage> damageList)
    {
        targetUnit.Battler.TakeDamage(damageList);
    }

    // アタック結果
    private void AttackResult()
    {
        if (targetUnit.Battler.Life <= 0)
        {
            StartCoroutine(SetBattleState(BattleState.BattleResult));
            StartCoroutine(BattleResult(sourceUnit, targetUnit));
        }
    }

    private IEnumerator SetBattleState(BattleState state)
    {
        // バトルステートを設定する処理（仮）
        yield return null;
    }

    private IEnumerator BattleResult(BattleUnit source, BattleUnit target)
    {
        // バトル結果の処理（仮）
        yield return null;
    }
}
