using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    enum State
    {
        Start,
        ActionSelection,
        ActionExecution,
        RunTurn,
        BattleOver,
    }

    State state;
    public UnityAction OnBattleEnd;
    [SerializeField] BattleCanvas battleCanvas;
    [SerializeField] MessageDialog messageDialog;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case State.Start:
                break;
            case State.ActionSelection:
                HandleActionSelection();
                break;
            case State.ActionExecution:
                break;
            case State.BattleOver:
                break;
        }
    }

    public void BattleStart(Battler player, Battler enemy)
    {
        state = State.Start;
        SetupBattle(player, enemy);
        battleCanvas.gameObject.SetActive(true);
        enemyUnit.SetMotion(BattleUnit.Motion.Jump);
    }

    public void SetupBattle(Battler player, Battler enemy)
    {
        Debug.Log("SetupBattle!!");
        enemyUnit.Setup(enemy);
        StartCoroutine(SetMessage($"{enemy.Base.Name} is coming!!"));
    }

    void HandleActionSelection()
    {

    }

    public void AttackTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(SetMessage("The player is waving his arms around."));
    }

    public void ItemTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(SetMessage("The player fished through his backpack but found nothing."));
    }

    public void TalkTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(SetMessage("The player tried talking to him, but he didn't respond."));
    }

    public void RunTurn()
    {
        state = State.ActionExecution;
        StartCoroutine(BattleEnd());
    }

    public IEnumerator SetMessage(string message)
    {
        Debug.Log("SetMessage!!");
        yield return messageDialog.TypeDialog(message);
    }

    public IEnumerator BattleEnd()
    {
        state = State.BattleOver;
        StartCoroutine(SetMessage("The player braced himself."));
        yield return new WaitForSeconds(0.8f);
        battleCanvas.gameObject.SetActive(false);
        OnBattleEnd?.Invoke();
    }
}
