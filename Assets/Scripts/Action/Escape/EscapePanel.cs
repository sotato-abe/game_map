using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EscapePanel : Panel
{
    [SerializeField] private TextMeshProUGUI probabilityText;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] private AttackSystem attackSystem;
    [SerializeField] BattleUnit enemyUnit;

    int lifeCost = 0;
    int batteryCost = 0;
    int soulCost = 0;

    int probability = 0;

    public void Update()
    {
        if (executeFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Escape();
            }
        }
    }

    private void OnEnable()
    {
        ProbabilityCalculation();
        CountEnegyCost();
    }

    private void ProbabilityCalculation()
    {
        int playerSPD = playerUnit.Battler.Speed.val;
        int enemySPD = enemyUnit.Battler.Speed.val;

        Debug.Log("Player SPD: " + playerSPD);
        Debug.Log("Enemy SPD: " + enemySPD);

        // 逃げる確率の計算
        // 逃げる確率 = (自分の素早さ / (自分の素早さ + 相手の素早さ)) * 100
        probability = (playerSPD * 100) / (playerSPD + enemySPD);
        probabilityText.SetText(probability.ToString() + "%");

        Debug.Log("Escape Probability: " + probability + "%");
    }

    private void CountEnegyCost()
    {
        lifeCost = Mathf.Max(1, playerUnit.Battler.Life / 10);
        batteryCost = Mathf.Max(1, playerUnit.Battler.Battery / 10);
        soulCost = playerUnit.Battler.Soul / 2;

        lifeCostText.SetText(lifeCost.ToString());
        batteryCostText.SetText(batteryCost.ToString());
        soulCostText.SetText(soulCost.ToString());
    }

    public void Escape()
    {
        if (executeFlg)
        {
            playerUnit.Battler.Life -= lifeCost;
            playerUnit.Battler.Battery -= batteryCost;
            playerUnit.Battler.Soul -= soulCost;

            if (Random.Range(0, 100) < probability)
            {
                // 逃げる成功
                attackSystem.ExecutePlayerEscape();
                playerUnit.UpdateEnegyUI();
                return;
            }
            else
            {
                // 逃げる失敗
                attackSystem.ExecuteEnemyAttack();
                playerUnit.UpdateEnegyUI();
            }
        }
        ProbabilityCalculation();
    }
}
