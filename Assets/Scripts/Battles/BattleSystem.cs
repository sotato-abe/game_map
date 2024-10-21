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
    [SerializeField] BattleCanvas battleCanvas;

    // [SerializeField] MessageDialog messageDialog;
    [SerializeField] EnemyDialog enemyDialog;
    // [SerializeField] BattleUnit playerUnit;

    public Vector3 enemyClosePosition = new Vector3(960 + 1250, 540 - 200, 0);
    public Vector3 enemyOpenPosition = new Vector3(960 + 750, 540 - 200, 0);


    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("BattleEnd!!");
            BattleEnd();
        }
    }

    // public void BattleStart(Battler player, Battler enemy)
    public void BattleStart()
    {
        // StartCoroutine(SetupBattle(player, enemy));
        SetupBattle();
        battleCanvas.gameObject.SetActive(true);
        // コルーチンを使って1秒かけて移動させる
        Vector3 enemyOpenPosition = new Vector3(960 + 750, 540 - 200, 0);
        StartCoroutine(MoveEnemyDialog(enemyOpenPosition));
    }

    private IEnumerator MoveEnemyDialog(Vector3 targetPosition)
    {
        // 現在の位置
        Vector3 startPosition = enemyDialog.transform.position;
        Debug.Log($"startPosition!!:{startPosition}");

        // 移動にかける時間
        float moveDuration = 0.1f;
        float elapsedTime = 0;

        // 1秒間かけて徐々に移動
        while (elapsedTime < moveDuration)
        {
            enemyDialog.transform.position = Vector3.MoveTowards(
                enemyClosePosition, targetPosition, (elapsedTime / moveDuration) * Vector3.Distance(startPosition, targetPosition)
            );

            elapsedTime += Time.deltaTime;
            yield return null; // 次のフレームまで待つ
        }

        // 最後に正確な位置に設定
        enemyDialog.transform.position = targetPosition;
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

    public void BattleEnd()
    {
        Vector3 enemyClosePosition = new Vector3(960 + 1250, 540 - 200, 0);
        StartCoroutine(MoveEnemyDialog(enemyClosePosition));
        // enemyDialog.transform.position = enemyClosePosition;
        // battleCanvas.gameObject.SetActive(false);
        // yield return StartCoroutine(SetMessage("The player braced himself."));
        // yield return new WaitForSeconds(0.5f);
        OnBattleEnd?.Invoke();
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
