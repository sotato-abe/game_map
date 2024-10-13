using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    bool isMoving = false;
    public int width;        // マップの幅
    public int height;       // マップの高さ

    public float encounterThreshold = 5.0f;
    public float distanceTraveled = 0.0f;
    private Vector3 lastPosition;

    [SerializeField] MapBase mapBase; //マップデータ(ここもゲームコントローラーから受け取るようにする)
    [SerializeField] LayerMask blockLayer;
    [SerializeField] LayerMask entryLayer;
    [SerializeField] LayerMask encountLayer;
    [SerializeField] Battler battler;
    private GenerateSeedMap generateSeedMap;

    public UnityAction OnEncount;

    public Battler Battler { get => battler; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // 仮でここで定義（後でマップ更新時に更新されるようにする）
        width = mapBase.MapWidth;
        height = mapBase.MapHeight;

        lastPosition = transform.position;
        generateSeedMap = FindObjectOfType<GenerateSeedMap>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == false)
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
                int entryNum = IsEntry(targetPosition);
                if (entryNum != 0)
                {
                    Debug.Log($"IsEntry");
                    generateSeedMap.ReloadMap(entryNum);
                }
                else
                {
                    Debug.Log($"Move");
                    StartCoroutine(Move(targetPosition));
                }
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

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

        if (Physics2D.OverlapCircle(transform.position, 0.2f, encountLayer))
        {

            if (Random.Range(0, 100) < 3)
            {
                Debug.Log("encount!!");
                OnEncount?.Invoke();
            }
        }

    }

    int IsEntry(Vector3 targetPos)
    {
        // 移動先にエントリーレイヤーがあったときはその方角の位置を返す
        if (Physics2D.OverlapCircle(targetPos, 0.2f, entryLayer))
        {
            Debug.Log($"isEntry!!x:{targetPos.x} y:{targetPos.y}");
            if (targetPos.x < 1)
            {
                return 1; // left
            }
            else if (targetPos.x > width - 1)
            {
                return 2; // right

            }
            else if (targetPos.y < 1)
            {
                return 3; // bottom
            }
            else if (targetPos.y > height - 1)
            {
                return 4; // top
            }
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
}
