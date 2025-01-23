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
    public Coordinate playerCoordinate;

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

    // フィールド移動時の方向を受け取る
    // カレント座標を更新する
    // 新しいフィールドのフィールドデータを取得する
    // フィールドを生成する処理（generateFieldMap）に受け渡す
    public void ChangeField(DirectionType outDirection)
    {
        DirectionType entryDirection = outDirection.GetOppositeDirection();
        Debug.Log($"GameController:ChangeField:{outDirection}>>>{entryDirection}");
        playerCoordinate = playerController.Battler.coordinate;
        Debug.Log($"StartPoint:Row: {playerCoordinate.row}, Col: {playerCoordinate.col}");
        generateFieldMap.ReloadMap(entryDirection);
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
