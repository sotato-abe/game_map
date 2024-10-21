using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FieldInfoSystem fieldInfoSystem;
    [SerializeField] Battler enemy;

    private void Start()
    {
        playerController.OnEncount += BattleStart;
        battleSystem.OnBattleEnd += BattleEnd;
    }

    public void BattleStart()
    {
        Debug.Log("BattleStart!!");
        playerController.SetMoveFlg(false);
        fieldInfoSystem.FieldDialogClose();
        battleSystem.gameObject.SetActive(true);
        // battleSystem.BattleStart(playerController.Battler, enemy);
        battleSystem.BattleStart();
    }

    public void BattleEnd()
    {
        Debug.Log("Game_Controller_BattleEnd");
        playerController.SetMoveFlg(true);
        battleSystem.gameObject.SetActive(false);
        fieldInfoSystem.FieldDialogOpen();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
