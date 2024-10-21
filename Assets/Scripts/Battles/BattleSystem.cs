using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;

    // [SerializeField] ActionController actionController;
    // [SerializeField] SkillDialog skillDialog;
    [SerializeField] MapDialog mapDialog;
    [SerializeField] BattleCanvas battleCanvas;

    // [SerializeField] MessageDialog messageDialog;
    // [SerializeField] BattleUnit enemyUnit;
    // [SerializeField] BattleUnit playerUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("BattleEnd!!");
            StartCoroutine(BattleEnd());
        }
    }

    // public void BattleStart(Battler player, Battler enemy)
    public void BattleStart()
    {
        // StartCoroutine(SetupBattle(player, enemy));
        SetupBattle();
        mapDialog.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(true);
    }

    // IEnumerator SetupBattle(Battler player, Battler enemy)
    public void SetupBattle()
    {
        Debug.Log("SetupBattle!!");
        // playerUnit.Setup(player);
        // enemyUnit.Setup(enemy);
        // actionController.Reset();
        // yield return messageDialog.TypeDialog("XX is coming!!");
        // yield return new WaitForSeconds(0.5f);
        // ActionDialogOpen();

        MapDialogClose();
        BattleCanvasOpen();
    }

    public void RunTurn()
    {

    }

    // public IEnumerator SetMessage(string message)
    // {
    //     Debug.Log("SetMessage!!");
    //     // yield return messageDialog.TypeDialog(message);
    // }

    public IEnumerator BattleEnd()
    {
        Debug.Log("battle canvas close");
        battleCanvas.gameObject.SetActive(false);
        mapDialog.gameObject.SetActive(true);
        // yield return StartCoroutine(SetMessage("The player braced himself."));
        yield return new WaitForSeconds(0.5f);
        OnBattleEnd?.Invoke();
    }

    public void MapDialogClose()
    {
        Debug.Log("map close");
        mapDialog.gameObject.SetActive(false);
    }

    public void BattleCanvasOpen()
    {
        Debug.Log("The enemy are coming!!");
        battleCanvas.gameObject.SetActive(true);
    }

    // public void SkillDialogOpen()
    // {
    //     Debug.Log("SkillDialog!!");
    //     StartCoroutine(SetMessage("The player braced himself."));
    //     actionController.gameObject.SetActive(false);
    //     skillDialog.Reset();
    //     skillDialog.gameObject.SetActive(true);
    // }

    // public void ActionDialogOpen()
    // {
    //     Debug.Log("ActionDialog!!");
    //     actionController.gameObject.SetActive(true);
    //     skillDialog.gameObject.SetActive(false);
    // }
}
