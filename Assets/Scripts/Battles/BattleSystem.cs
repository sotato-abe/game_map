using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;

    [SerializeField] ActionController actionController;
    [SerializeField] SkillDialog skillDialog;
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleUnit playerUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        actionController.gameObject.SetActive(true);
        StartCoroutine(SetupBattle(player, enemy));
    }

    IEnumerator SetupBattle(Battler player, Battler enemy)
    {
        Debug.Log("SetupBattle!!");
        playerUnit.Setup(player);
        enemyUnit.Setup(enemy);

        actionController.Reset();
        yield return messageDialog.TypeDialog("XX is coming!!");
        yield return new WaitForSeconds(0.5f);
        ActionDialogOpen();
    }

    public void RunTurn()
    {

    }

    public IEnumerator SetMessage(string message)
    {
        Debug.Log("SetMessage!!");
        yield return messageDialog.TypeDialog(message);
    }

    public IEnumerator BattleEnd()
    {
        yield return StartCoroutine(SetMessage("The player braced himself."));
        yield return new WaitForSeconds(0.5f);
        OnBattleEnd?.Invoke();
    }


    public void SkillDialogOpen()
    {
        Debug.Log("SkillDialog!!");
        StartCoroutine(SetMessage("The player braced himself."));
        actionController.gameObject.SetActive(false);
        skillDialog.Reset();
        skillDialog.gameObject.SetActive(true);
    }

    public void ActionDialogOpen()
    {
        Debug.Log("ActionDialog!!");
        actionController.gameObject.SetActive(true);
        skillDialog.gameObject.SetActive(false);
    }
}