using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    private BattleUnit sourceUnit;
    private BattleUnit targetUnit;

    public void ExecuteAttack(BattleUnit sourceUnit, BattleUnit targetUnit, List<Skill> skills)
    {
        this.sourceUnit = sourceUnit;
        this.targetUnit = targetUnit;

        List<Damage> damageList = CalculateDamage(skills);
        TakeDamage(damageList);
        AttackResult();
    }

    // ダメージを計算
    private List<Damage> CalculateDamage(List<Skill> skills)
    {
        Dictionary<SkillType, int> damageDict = new Dictionary<SkillType, int>();

        foreach (Skill skill in skills)
        {
            if (damageDict.ContainsKey(skill.Type))
            {
                damageDict[skill.Type] += skill.Val;
            }
            else
            {
                damageDict[skill.Type] = skill.Val;
            }
        }

        List<Damage> damageList = new List<Damage>();
        foreach (var pair in damageDict)
        {
            damageList.Add(new Damage { type = pair.Key, val = pair.Value });
        }

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
