using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] FieldInfoSystem fieldInfoSystem;
    [SerializeField] Battler enemy;

    private void Start()
    {
        Debug.Log("[Game_Controller]:Start!!");
        playerController.OnEncount += BattleStart;
        battleSystem.OnBattleEnd += BattleEnd;
        StartCoroutine(messageDialog.TypeDialog("game start"));
    }

    public void BattleStart()
    {
        Debug.Log("[Game_Controller]:BattleStart!!");
        playerController.SetMoveFlg(false);
        fieldInfoSystem.FieldDialogClose();
        battleSystem.gameObject.SetActive(true);
        enemy.Init();
        battleSystem.BattleStart(playerController.Battler, enemy);

        // battleSystem.BattleStart();
    }

    public void BattleEnd()
    {
        Debug.Log("[Game_Controller]:BattleEnd");
        playerController.SetMoveFlg(true);
        battleSystem.gameObject.SetActive(false);
        fieldInfoSystem.FieldDialogOpen();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
