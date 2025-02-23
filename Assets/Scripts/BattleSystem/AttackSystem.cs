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
            this.targetUnit.UpdateUI();
            this.targetUnit.SetMotion(MotionType.Shake);
        }
        AttackResult();
    }

    // ダメージを計算
    private List<Damage> CalculateDamage()
    {
        Dictionary<(AttackType, int), int> damageDict = new Dictionary<(AttackType, int), int>();

        // TODO : ちゃんとPlayer判定をする
        IEnumerable<Skill> skills = sourceUnit.Battler.Base.Name == "Sola"
            ? attackPanel.ActivateEquipments()
            : sourceUnit.Battler.Base.Equipments
                .Where(e => Random.Range(0, 100) < e.Base.Probability)
                .SelectMany(e => e.Base.SkillList);

        // スキルごとのダメージを計算
        foreach (Skill skill in skills)
        {
            // EnegyとEnchantを共通処理
            foreach (var enegy in skill.Enegys)
            {
                // DamageのAttackTypeはEnegyで固定
                var key = (AttackType.Enegy, (int)enegy.type);

                // Dictionaryにエネジーのダメージを追加
                if (damageDict.ContainsKey(key))
                    damageDict[key] += enegy.val;
                else
                    damageDict[key] = enegy.val;
            }
        }

        // Dictionary を List に変換して返す
        return damageDict.Select(kvp => new Damage(AttackType.Enegy, kvp.Key.Item2, kvp.Value)).ToList();
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
