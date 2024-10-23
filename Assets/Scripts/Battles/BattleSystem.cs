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

    // [SerializeField] ActionController actionController;
    [SerializeField] BattleCanvas battleCanvas;

    [SerializeField] MessageDialog messageDialog;
    [SerializeField] EnemyDialog enemyDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    public Vector3 enemyClosePosition = new Vector3(960 + 1250, 540 - 200, 0);
    public Vector3 enemyOpenPosition = new Vector3(960 + 750, 540 - 200, 0);


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
        StartCoroutine(OpenEnemyDialog(enemyDialog));
    }

    public void SetupBattle(Battler player, Battler enemy)
    {
        Debug.Log("SetupBattle!!");
        enemyUnit.Setup(enemy);
        Debug.Log($"player:{playerUnit.name}/enemy:{enemyUnit}");
        // actionController.Reset();
        StartCoroutine(SetMessage($"{enemy.Base.Name} is coming!!"));
        // yield return messageDialog.TypeDialog("XX is coming!!");
        // yield return new WaitForSeconds(0.5f);
        BattleCanvasOpen();
    }

    private IEnumerator OpenEnemyDialog(EnemyDialog targetObject)
    {
        Debug.Log("OpenEnemyDialog!!");
        float initialBounceHeight = 40f;  // 初めのバウンドの高さ
        float dampingFactor = 0.5f;      // 減衰率（バウンドの大きさがどれくらいずつ減るか）
        float gravity = 5000f;            // 重力の強さ
        float groundY = enemyOpenPosition.y;  // 地面のY座標（開始位置に基づく）
        float currentBounceHeight = initialBounceHeight;
        float verticalVelocity = Mathf.Sqrt(2 * gravity * currentBounceHeight);
        bool isFalling = true;

        while (currentBounceHeight >= 0.1f)  // バウンドが小さくなって停止するまでループ
        {
            // バウンド中の垂直方向の動き
            if (isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;  // 重力で速度を減少させる
                targetObject.transform.position += Vector3.up * verticalVelocity * Time.deltaTime;  // 垂直方向に移動

                // 地面に到達したらバウンド
                if (targetObject.transform.position.y <= groundY)
                {
                    currentBounceHeight *= dampingFactor;  // バウンドの高さを減衰
                    verticalVelocity = Mathf.Sqrt(2 * gravity * currentBounceHeight);  // 新しいバウンドの速度を計算
                    isFalling = false;  // 上昇に切り替える
                }
            }
            else
            {
                // 上昇中の処理
                verticalVelocity -= gravity * Time.deltaTime;  // 上昇中の速度を減少
                targetObject.transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                // 上昇が終わったら落下に切り替える
                if (verticalVelocity <= 0)
                {
                    isFalling = true;
                }
            }

            yield return null;  // 次のフレームまで待つ
        }

        // 最終的な位置を調整（小さなバウンドを終えた後に地面に戻す）
        targetObject.transform.position = new Vector3(targetObject.transform.position.x, groundY, targetObject.transform.position.z);
    }

    void HandleActionSelection()
    {

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
        // battleCanvas.gameObject.SetActive(false);
        StartCoroutine(SetMessage("The player braced himself."));
        yield return new WaitForSeconds(0.8f);
        battleCanvas.gameObject.SetActive(false);
        OnBattleEnd?.Invoke();
    }

    public void BattleCanvasOpen()
    {
        Debug.Log("The enemy are coming!!");
        battleCanvas.gameObject.SetActive(true);
    }

    // public void ActionDialogOpen()
    // {
    //     actionController.gameObject.SetActive(true);
    // }
}
