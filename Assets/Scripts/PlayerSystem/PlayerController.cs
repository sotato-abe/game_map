using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// マップ上のプレイヤーの動きを制御する
// フィールド移動を検知しゲームコントローラーに移動をリクエストする
public class PlayerController : MonoBehaviour
{
    Animator animator;
    bool isMoving = false;
    bool canMove = true;
    public int width; // マップの幅
    public int height; // マップの高さ

    public int encounterThreshold = 1;
    public float distanceTraveled = 0.0f;
    private Vector3 lastPosition;
    [SerializeField] MapBase mapBase; //マップデータ(ここもゲームコントローラーから受け取るようにする)
    [SerializeField] LayerMask blockLayer;
    [SerializeField] LayerMask entryLayer;
    [SerializeField] LayerMask areaLayer;
    [SerializeField] LayerMask encountLayer;
    [SerializeField] TargetPin targetPin;
    [SerializeField] PlayerBattler battler;
    [SerializeField] BattleUnit playerUnit;

    public UnityAction OnEncount;
    public UnityAction OnReserve;
    public delegate void ChangeFieldDelegate(DirectionType fieldId);
    public event ChangeFieldDelegate ChangeField;

    public Coordinate currentField;

    public PlayerBattler Battler { get => battler; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        battler.Init();
        playerUnit.Setup(battler);

        StartCoroutine(playerUnit.SetTalkMessage("start.."));
        // 仮でここで定義（後でマップ更新時に更新されるようにする）
        width = mapBase.MapWidth;
        height = mapBase.MapHeight;

        lastPosition = transform.position;
    }

    // フィールド上のキャラクターのモーションを制御
    void Update()
    {
        if (isMoving == false && canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float moveDirection = Input.GetAxis("Horizontal");

            if (x != 0 || y != 0)
            {
                Vector3 currentScale = transform.localScale;
                distanceTraveled += Vector3.Distance(transform.position, lastPosition);
                lastPosition = transform.position;

                if (moveDirection > 0 && currentScale.x < 0)
                {
                    // 反転を元に戻す
                    transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
                }
                // 左に移動する場合
                else if (moveDirection < 0 && currentScale.x > 0)
                {
                    // キャラクターを反転させる
                    transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
                }
                animator.SetFloat("inputX", x);
                animator.SetFloat("inputY", y);
                Vector3 targetPosition = transform.position + (new Vector3(x, y, 0));
                DirectionType outDirection = IsEntry(targetPosition);
                if (outDirection != 0)
                {
                    // フィールドを移動
                    ChangeField?.Invoke(outDirection);
                }
                else
                {
                    StartCoroutine(Move(targetPosition));
                }
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        if (canMove && Input.GetKeyDown(KeyCode.Return))
        {
            OnReserve?.Invoke();
        }
    }

    // フィールド上のキャラクターの移動を制御
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        if (IsWalkable(targetPos) == false)
        {
            isMoving = false;
            yield break;
        }
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        CheckForEncount();
    }

    void CheckForEncount()
    {
        int randamEncounterThreshold = Random.Range(0, 100);
        if (Physics2D.OverlapCircle(transform.position, 0.2f, areaLayer))
        {
            if (randamEncounterThreshold < encounterThreshold * 2)
            {
                OnEncount?.Invoke();
            }
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.2f, encountLayer))
        {
            if (randamEncounterThreshold < encounterThreshold)
            {
                OnEncount?.Invoke();
            }
        }
    }

    DirectionType IsEntry(Vector3 targetPos)
    {
        // 移動先にエントリーレイヤーがあったときはその方角の位置を返す
        if (Physics2D.OverlapCircle(targetPos, 0.2f, entryLayer))
        {
            if (targetPos.x < 1) return DirectionType.Left;  // left
            if (targetPos.x > width - 1) return DirectionType.Right;  // right
            if (targetPos.y < 1) return DirectionType.Bottom;  // bottom
            if (targetPos.y > height - 1) return DirectionType.Top;  // top
        }
        return 0;
    }

    bool IsWalkable(Vector3 targetPos)
    {
        // 移動先にブロックレイヤーがあったときはfalseになる
        if (targetPos.x < 1 || targetPos.x > width || targetPos.y < 1 || targetPos.y > height)
        {
            return false;
        }
        else
        {
            return Physics2D.OverlapCircle(targetPos, 0.2f, blockLayer) == false;
        }
    }

    public void SetMoveFlg(bool flg)
    {
        canMove = flg;
    }
}
