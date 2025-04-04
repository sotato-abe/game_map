using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] FieldPlayerController fieldPlayerController;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FieldInfoSystem fieldInfoSystem;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] GenerateFieldMap generateFieldMap;

    //　プレイヤーの現在座標を保持する変数
    //　後々１つのクラスとして独立させる
    public Coordinate playerCoordinate;

    Battler enemy;

    private void Start()
    {
        fieldPlayerController.OnReserve += ReserveStart;
        fieldPlayerController.OnEncount += BattleStart;
        fieldPlayerController.ChangeField += ChangeField;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        messagePanel.AddMesageList("game start");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        playerCoordinate = fieldPlayerController.Battler.coordinate;
    }

    // フィールド移動時の方向を受け取る
    // カレント座標を更新する
    // 新しいフィールドのフィールドデータを取得する
    // フィールドを生成する処理（generateFieldMap）に受け渡す
    public void ChangeField(DirectionType outDirection)
    {
        DirectionType entryDirection = outDirection.GetOppositeDirection();
        if (outDirection == DirectionType.Top)
            playerCoordinate.row = playerCoordinate.row - 1;
        if (outDirection == DirectionType.Bottom)
            playerCoordinate.row = playerCoordinate.row + 1;
        if (outDirection == DirectionType.Right)
            playerCoordinate.col = playerCoordinate.col + 1;
        if (outDirection == DirectionType.Left)
            playerCoordinate.col = playerCoordinate.col - 1;
        generateFieldMap.ReloadMap(entryDirection, playerCoordinate);
    }

    public void ReserveStart()
    {
        Debug.Log("ReserveStart");
        fieldPlayerController.SetMoveFlg(false);
        battleSystem.gameObject.SetActive(false);
        reserveSystem.ReserveStart();
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void ReserveEnd()
    {
        Debug.Log("ReserveEnd");
        fieldPlayerController.SetMoveFlg(true);
        reserveSystem.gameObject.SetActive(false);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void BattleStart()
    {
        Debug.Log("BattleStart");
        fieldPlayerController.SetMoveFlg(false);
        // fieldInfoSystem.FieldDialogClose();
        reserveSystem.gameObject.SetActive(false);
        enemy = fieldInfoSystem.GetRandomEnemy();
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(fieldPlayerController.Battler, enemy);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void BattleEnd()
    {
        Debug.Log("BattleEnd");
        battleSystem.gameObject.SetActive(false);
        fieldPlayerController.SetMoveFlg(true);
        // fieldInfoSystem.FieldDialogOpen();
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }
}
