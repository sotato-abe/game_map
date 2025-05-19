using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class EscapePanel : Panel
{
    [SerializeField] private TextMeshProUGUI probabilityText;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] Image runningBar1;
    [SerializeField] Image runningBar2;
    [SerializeField] Image runningBar3;
    [SerializeField] Image runningBar4;

    [SerializeField] Color activeColor = new Color(133, 10, 240, 255);
    [SerializeField] Color stopColor = new Color(0, 0, 0, 200);

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
                StartCoroutine(Escape());
            }
        }
    }

    private void OnEnable()
    {
        ProbabilityCalculation();
        CountEnegyCost();
        RunningOff();
    }

    private void ProbabilityCalculation()
    {
        int playerSPD = playerUnit.Battler.Speed.val;
        int enemySPD = enemyUnit.Battler.Speed.val;

        // 逃げる確率の計算
        // 逃げる確率 = (自分の素早さ / (自分の素早さ + 相手の素早さ)) * 100
        probability = (playerSPD * 100) / (playerSPD + enemySPD);
        probabilityText.SetText(probability.ToString() + "%");
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

    private IEnumerator Escape()
    {
        if (executeFlg)
        {
            playerUnit.Battler.Life -= lifeCost;
            playerUnit.Battler.Battery -= batteryCost;
            playerUnit.Battler.Soul -= soulCost;

            yield return StartCoroutine(RunningCoroutine());

            if (Random.Range(0, 100) < probability)
            {
                // 逃げる成功
                attackSystem.ExecutePlayerEscape();
                playerUnit.UpdateEnegyUI();
            }
            else
            {
                // 逃げる失敗
                attackSystem.ExecuteEnemyAttack();
                playerUnit.UpdateEnegyUI();
            }
        }
        RunningOff();
        ProbabilityCalculation();
    }

    private IEnumerator RunningCoroutine()
    {
        // escapeBarは0から3までのランダムな値
        int escapeBar = Random.Range(1, 5);
        for (int i = 0; i < escapeBar; i++)
        {
            if (i == 0)
            {
                runningBar1.color = activeColor;
            }
            else if (i == 1)
            {
                runningBar2.color = activeColor;
            }
            else if (i == 2)
            {
                runningBar3.color = activeColor;
            }
            else if (i == 3)
            {
                runningBar4.color = activeColor;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RunningOff()
    {
        // Corutinを停止
        StopAllCoroutines();
        runningBar1.color = stopColor;
        runningBar2.color = stopColor;
        runningBar3.color = stopColor;
        runningBar4.color = stopColor;
    }
}
