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
    [SerializeField] TradeSystem tradeSystem;
    [SerializeField] FieldSystem fieldSystem;
    [SerializeField] ConfigSystem configSystem;
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] MessagePanel messagePanel;

    //　プレイヤーの現在座標を保持する変数
    //　後々１つのクラスとして独立させる
    public Vector2Int playerCoordinate;

    Battler enemy;

    private void Awake()
    {
        playerBattler.Init();
        fieldPlayer.SetUp(playerBattler); // フィールドプレイヤーの初期化
        fieldSystem.Setup(playerBattler); // フィールドシステムの初期化
        fieldSystem.SetFieldPanelData();
        fieldSystem.OnReserve += ReserveStart;
        fieldSystem.OnEncount += BattleStart;

        fieldPlayer.OnTradeStart += TradeStart;
        tradeSystem.OnTradeEnd += TradeEnd;

        configSystem.OnConfigOpen += ConfigStart;
        configSystem.OnConfigClose += ConfigEnd;

        playerUnit.Setup(playerBattler); // プレイヤーのバトルユニットの初期化
        playerUnit.SetTalkMessage("よし、はじめるか。", PanelType.Default);

        playerCoordinate = playerBattler.coordinate;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        messagePanel.AddMessage(MessageIconType.System, "Hallo World");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
    }

    public void TradeStart(BuildingType type)
    {
        // Debug.Log("TradeStart");
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        configSystem.SetActive(false);
        tradeSystem.TradeStart(type);
    }

    public void TradeEnd()
    {
        // Debug.Log("TradeEnd");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        configSystem.SetActive(true);
        fieldPlayer.SetMoveFlg(true);
    }

    public void ReserveStart()
    {
        // Debug.Log("ReserveStart");
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        configSystem.SetActive(false);
        reserveSystem.ReserveStart();
    }

    public void ReserveEnd()
    {
        // Debug.Log("ReserveEnd");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetMoveFlg(true);
        configSystem.SetActive(true);
    }

    public void BattleStart()
    {
        // Debug.Log("BattleStart");
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        configSystem.SetActive(false);
        List<Battler> enemyGroup = fieldSystem.GetEnemyGruop();
        battleSystem.SetBattle(enemyGroup);
    }

    public void BattleEnd()
    {
        // Debug.Log("BattleEnd");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        configSystem.SetActive(true);
        fieldPlayer.SetMoveFlg(true);
    }

    public void ConfigStart()
    {
        // Debug.Log("ConfigStart");
        ageTimePanel.SetTimeSpeed(TimeState.Live);
        fieldPlayer.SetMoveFlg(false);
    }

    public void ConfigEnd()
    {
        // Debug.Log("ConfigEnd");
        ageTimePanel.SetTimeSpeed(TimeState.Fast);
        fieldPlayer.SetMoveFlg(true);
    }
}
