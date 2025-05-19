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
    [SerializeField] ConfigSystem configSystem;

    //　プレイヤーの現在座標を保持する変数
    //　後々１つのクラスとして独立させる
    public Vector2Int playerCoordinate;

    Battler enemy;

    private void Awake()
    {
        playerBattler.Init();
        fieldSystem.Setup(playerBattler); // フィールドシステムの初期化
        fieldSystem.OnReserve += ReserveStart;
        fieldSystem.OnEncount += BattleStart;
        configSystem.OnConfigOpen += ConfigStart;
        configSystem.OnConfigClose += ConfigEnd;

        playerUnit.Setup(playerBattler); // プレイヤーのバトルユニットの初期化
        playerUnit.SetTalkMessage("よし。。");

        playerCoordinate = playerBattler.coordinate;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        messagePanel.AddMessage(MessageIconType.System, "Hallo World");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void ReserveStart()
    {
        // Debug.Log("ReserveStart");
        battleSystem.gameObject.SetActive(false);
        configSystem.SetActive(false);
        reserveSystem.ReserveStart();
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void ReserveEnd()
    {
        // Debug.Log("ReserveEnd");
        fieldPlayer.SetMoveFlg(true);
        reserveSystem.gameObject.SetActive(false);
        configSystem.SetActive(true);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void BattleStart()
    {
        // Debug.Log("BattleStart");
        reserveSystem.gameObject.SetActive(false);
        configSystem.SetActive(false);
        enemy = fieldSystem.GetEnemy();
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(playerBattler, enemy);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void BattleEnd()
    {
        // Debug.Log("BattleEnd");
        battleSystem.gameObject.SetActive(false);
        configSystem.SetActive(true);
        fieldPlayer.SetMoveFlg(true);
        fieldSystem.RemoveEnemy();
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void ConfigStart()
    {
        fieldPlayer.SetMoveFlg(false);
        ageTimePanel.SetTimeSpeed(TimeState.Live);
    }

    public void ConfigEnd()
    {
        fieldPlayer.SetMoveFlg(true);
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }
}
