using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FieldInfoSystem fieldInfoSystem;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] GenerateFieldMap generateFieldMap;
    public Coordinate currentField;

    Battler enemy;

    private void Start()
    {
        playerController.OnReserve += ReserveStart;
        playerController.OnEncount += BattleStart;
        playerController.ChangeField += ChangeField;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        reserveSystem.ActionPanel.SetPanelValidity(0.2f);
        StartCoroutine(messagePanel.TypeDialog("game start"));
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void ChangeField(DirectionType entryNum)
    {
        // 1 left
        // 2 right
        // 3 bottom
        // 4 top
        generateFieldMap.ReloadMap(entryNum);

        Debug.Log($"GameController:ChangeField:{entryNum}");
        currentField = playerController.Battler.GetBirthCoordinate();
        Debug.Log($"StartPoint:Row: {currentField.row}, Col: {currentField.col}");
    }

    public void ReserveStart()
    {
        playerController.SetMoveFlg(false);
        battleSystem.gameObject.SetActive(false);
        reserveSystem.gameObject.SetActive(true);
        reserveSystem.ReserveStart(playerController.Battler);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void ReserveEnd()
    {
        playerController.SetMoveFlg(true);
        reserveSystem.gameObject.SetActive(false);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void BattleStart()
    {
        Debug.Log("BattleStart");
        playerController.SetMoveFlg(false);
        fieldInfoSystem.FieldDialogClose();
        reserveSystem.gameObject.SetActive(false);
        enemy = fieldInfoSystem.GetRandomEnemy();
        enemy.Init();
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(playerController.Battler, enemy);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void BattleEnd()
    {
        Debug.Log("BattleEnd");
        battleSystem.gameObject.SetActive(false);
        playerController.SetMoveFlg(true);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    void Update()
    {

    }
}
