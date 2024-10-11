using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    bool isMoving = false;
    public float encounterThreshold = 5.0f;
    public float distanceTraveled = 0.0f;
    private Vector3 lastPosition;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] LayerMask encountLayer;
    [SerializeField] Battler battler;
    public UnityAction OnEncount;

    public Battler Battler { get => battler; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
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
                StartCoroutine(Move(new Vector3(x, y, 0)));
                animator.SetBool("isMoving", true);
            }else {
                animator.SetBool("isMoving", false);
            }
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPos = transform.position + direction;
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

    bool IsWalkable(Vector3 targetPos)
    {
        // 移動先にブロックレイヤーがあったときはfalseになる
        return Physics2D.OverlapCircle(targetPos, 0.2f, blockLayer) == false;
    }
}
