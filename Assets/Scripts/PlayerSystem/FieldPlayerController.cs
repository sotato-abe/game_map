using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// マップ上のプレイヤーの動きを制御する
// フィールド移動を検知しゲームコントローラーに移動をリクエストする
public class FieldPlayerController : MonoBehaviour
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
    [SerializeField] PlayerBattler battler;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] float moveSpeed = 3f;

    public UnityAction OnEncount;
    public UnityAction OnReserve;
    Coroutine currentMoveCoroutine;
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

    public void MoveToTargetPin(Vector3 targetPos)
    {
        Debug.Log("MoveToTargetPin called with targetPos: " + targetPos);
        Vector2Int target = new Vector2Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y));
        if (IsWalkable(targetPos) == false) return;

        // 左右反転（横移動のときだけでOK）
        // 進行方向を取得
        Vector3 dir = targetPos - transform.position;
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Vector2Int start = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        List<Vector2Int> path = FindPath(start, target);
        if (path != null && path.Count > 1)
        {
            // 前の移動を止める
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
                isMoving = false;
                animator.SetBool("isMoving", false);
            }

            // 新しい移動を開始
            currentMoveCoroutine = StartCoroutine(MoveAlongPath(path));
        }
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Vector2Int>();
        var grid = new Dictionary<Vector2Int, Node>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                bool walkable = IsWalkable(new Vector3(x, y));
                grid[pos] = new Node(pos, walkable);
            }
        }

        Node startNode = grid[start];
        Node targetNode = grid.ContainsKey(target) ? grid[target] : null;

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => a.F.CompareTo(b.F));
            Node current = openList[0];
            openList.RemoveAt(0);
            closedList.Add(current.Position);

            if (current.Position == target)
            {
                // パスを逆にたどってリスト化
                List<Vector2Int> path = new();
                while (current != null)
                {
                    path.Insert(0, current.Position);
                    current = current.Parent;
                }
                return path;
            }

            foreach (Vector2Int offset in new Vector2Int[] {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        })
            {
                Vector2Int neighborPos = current.Position + offset;
                if (!grid.ContainsKey(neighborPos)) continue;

                Node neighbor = grid[neighborPos];
                if (!neighbor.Walkable || closedList.Contains(neighbor.Position)) continue;

                int tentativeG = current.G + 1;
                bool isInOpenList = openList.Contains(neighbor);

                if (!isInOpenList || tentativeG < neighbor.G)
                {
                    neighbor.G = tentativeG;
                    neighbor.H = Mathf.Abs(target.x - neighbor.Position.x) + Mathf.Abs(target.y - neighbor.Position.y);
                    neighbor.Parent = current;

                    if (!isInOpenList)
                        openList.Add(neighbor);
                }
            }
        }

        // ターゲットに届かなかった場合、ここで一番近いノードを探す（optional）
        return null;
    }

    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isMoving = true;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 currentPos = new Vector3(path[i - 1].x, path[i - 1].y, 0);
            Vector3 targetPos = new Vector3(path[i].x, path[i].y, 0);
            Vector3 dir = (targetPos - currentPos).normalized;

            // アニメーターの向き設定
            animator.SetFloat("inputX", dir.x);
            animator.SetFloat("inputY", dir.y);
            animator.SetBool("isMoving", true);

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            yield return null;
        }

        animator.SetBool("isMoving", false);
        isMoving = false;
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
