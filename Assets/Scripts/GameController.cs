using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    // [SerializeField] Battler enemy;

    private void Start()
    {
        playerController.OnEncount += BattleStart;
        battleSystem.OnBattleEnd += BattleEnd;
    }

    public void BattleStart()
    {
        Debug.Log("BattleStart!!");
        // playerController.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        // battleSystem.BattleStart(playerController.Battler, enemy);
        battleSystem.BattleStart();
    }

    public void BattleEnd()
    {
        // playerController.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
