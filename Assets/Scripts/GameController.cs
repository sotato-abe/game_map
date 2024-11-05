using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] ReserveSystem reserveSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FieldInfoSystem fieldInfoSystem;

    Battler enemy;

    private void Start()
    {
        playerController.OnReserve += ReserveStart;
        playerController.OnEncount += BattleStart;
        reserveSystem.OnReserveEnd += ReserveEnd;
        battleSystem.OnBattleEnd += BattleEnd;
        reserveSystem.ActionPanel.SetActionValidity(0.2f);
    }

    public void ReserveStart()
    {
        playerController.SetMoveFlg(false);
        battleSystem.gameObject.SetActive(false);
        reserveSystem.gameObject.SetActive(true);
        reserveSystem.ReserveStart(playerController.Battler);
    }

    public void ReserveEnd()
    {
        playerController.SetMoveFlg(true);
        reserveSystem.gameObject.SetActive(false);
        fieldInfoSystem.FieldDialogOpen();
    }

    public void BattleStart()
    {
        playerController.SetMoveFlg(false);
        fieldInfoSystem.FieldDialogClose();
        reserveSystem.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        enemy = fieldInfoSystem.GetRandomEnemy();
        enemy.Init();
        battleSystem.BattleStart(playerController.Battler, enemy);
    }

    public void BattleEnd()
    {
        battleSystem.gameObject.SetActive(false);
        fieldInfoSystem.FieldDialogOpen();
        playerController.SetMoveFlg(true);
    }

    void Update()
    {

    }
}
