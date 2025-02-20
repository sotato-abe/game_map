using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    private BattleUnit sourceUnit;
    private BattleUnit targetUnit;

    public void ExecuteAttack(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        this.sourceUnit = sourceUnit;
        this.targetUnit = targetUnit;

        List<Damage> damageList = CalculateDamage();
        Debug.Log(damageList.Count);
        TakeDamage(damageList);
        if(0 < damageList.Count)
        {
            this.targetUnit.UpdateUI();
            this.targetUnit.SetMotion(MotionType.Shake);
        }
        AttackResult();
    }

    // ダメージを計算
    private List<Damage> CalculateDamage()
    {
        List<Skill> skills = new List<Skill>();
        Dictionary<SkillType, Damage> damageDict = new Dictionary<SkillType, Damage>();

        // 各装備のスキルをリストに追加
        foreach (Equipment equipment in sourceUnit.Battler.Base.Equipments)
        {
            // Error skillを追加できていない。
            skills.AddRange(equipment.Base.SkillList);
        }

        // スキルごとのダメージを計算
        foreach (Skill skill in skills)
        {
            if (damageDict.ContainsKey(skill.Type))
            {
                damageDict[skill.Type].Val += skill.Val; // 既存のダメージに加算
            }
            else
            {
                damageDict[skill.Type] = new Damage(skill.Type, skill.Val); // 新規追加
            }
        }

        // Dictionary から List に変換して返す
        return new List<Damage>(damageDict.Values);
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
