using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerBattler playerBattler;
    [SerializeField] PlayerUnit playerUnit;
    [SerializeField] FieldPlayer fieldPlayer;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] FieldSystem fieldSystem;

    //　プレイヤーの現在座標を保持する変数
    //　後々１つのクラスとして独立させる
    public Coordinate playerCoordinate;

    Battler enemy;

    private void Awake()
    {
        playerBattler.Init();
        fieldSystem.Setup(playerBattler); // フィールドシステムの初期化
        fieldSystem.OnReserve += ReserveStart;
        fieldSystem.OnEncount += BattleStart;
        
        playerUnit.Setup(playerBattler); // プレイヤーのバトルユニットの初期化
        playerUnit.SetTalkMessage("start..");

        playerCoordinate = playerBattler.coordinate;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        messagePanel.AddMesageList("game start");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void ReserveStart()
    {
        // Debug.Log("ReserveStart");
        battleSystem.gameObject.SetActive(false);
        reserveSystem.ReserveStart();
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void ReserveEnd()
    {
        // Debug.Log("ReserveEnd");
        fieldPlayer.SetMoveFlg(true);
        reserveSystem.gameObject.SetActive(false);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void BattleStart()
    {
        // Debug.Log("BattleStart");
        reserveSystem.gameObject.SetActive(false);
        enemy = fieldSystem.GetEnemy();
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(playerBattler, enemy);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void BattleEnd()
    {
        // Debug.Log("BattleEnd");
        battleSystem.gameObject.SetActive(false);
        fieldPlayer.SetMoveFlg(true);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }
}
