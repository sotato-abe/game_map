using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderSystem : MonoBehaviour
{
    [SerializeField] TurnBattlerUnit turnBattlerUnitPrefab;
    [SerializeField] GameObject turnBattlerList;
    [SerializeField] BattleSystem battleSystem;
    int intervalBetweenBattlers = 10;

    private bool isActive = true;

    public void SetUpBattlerTurns(List<Battler> battlers)
    {
        battlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        foreach (Transform child in turnBattlerList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Battler battler in battlers)
        {
            int interval = (int)(100 / battler.Speed) * intervalBetweenBattlers;
            int currentPosition = 50;

            for (int i = interval; i < 700; i += interval)
            {
                TurnBattlerUnit battlerUnit = Instantiate(turnBattlerUnitPrefab, turnBattlerList.transform);
                battlerUnit.gameObject.SetActive(true);
                battlerUnit.Initialize(battler, this);

                RectTransform rectTransform = battlerUnit.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0f, 0.5f);
                rectTransform.anchorMax = new Vector2(0f, 0.5f);
                rectTransform.pivot = new Vector2(0f, 0.5f);

                rectTransform.anchoredPosition = new Vector2(currentPosition + i, 0f);

                battlerUnit.StartMoving();
            }
        }
        isActive = true;
    }

    public void ExecuteTurn(TurnBattlerUnit battlerUnit)
    {
        Debug.Log($"ターンを実行: {battlerUnit.Battler.Base.Name}");
        StartCoroutine(battleSystem.SetBattleState(BattleState.ActionSelection));
        isActive = false;

        // ターンが完了したバトラーオブジェクトを再配置
        if (turnBattlerList.transform.childCount > 1)
        {
            foreach (Transform child in turnBattlerList.transform)
            {
                TurnBattlerUnit otherBattlerUnit = child.GetComponent<TurnBattlerUnit>();

                if (otherBattlerUnit != null)
                {
                    otherBattlerUnit.StopMoving();
                }
            }
        }
    }

    public void SetActive(bool flg)
    {
        isActive = flg;
        Debug.Log(isActive);
        if (isActive && turnBattlerList.transform.childCount > 1)
        {
            foreach (Transform child in turnBattlerList.transform)
            {
                TurnBattlerUnit battlerUnit = child.GetComponent<TurnBattlerUnit>();
                if (battlerUnit != null && !battlerUnit.IsActive)
                {
                    battlerUnit.StartMoving();
                }
            }
        }
    }
}
