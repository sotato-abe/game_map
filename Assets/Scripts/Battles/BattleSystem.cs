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
        MoveSelection,
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
            case State.MoveSelection:
                break;
            case State.RunTurn:
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
        StartCoroutine(enemyUnit.OpenEnemyDialog());
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


    public void RunTurn()
    {
        StartCoroutine(BattleEnd());
    }

    public IEnumerator SetMessage(string message)
    {
        Debug.Log("SetMessage!!");
        yield return messageDialog.TypeDialog(message);
    }

    public IEnumerator BattleEnd()
    {
        StartCoroutine(SetMessage("The player braced himself."));
        yield return new WaitForSeconds(0.8f);
        battleCanvas.gameObject.SetActive(false);
        OnBattleEnd?.Invoke();
    }
}
