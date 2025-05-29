using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FieldCharacter の基本的な動きを定義するクラス
public class FieldCharacter : MonoBehaviour
{
    [SerializeField] public Animator animator;
    public Battler Battler; // このフィールドキャラクターに関連付けられたバトラー
    public Animator Animator => animator;

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public virtual void SetUp(Battler battler = null)
    {
        this.Battler = battler;
    }

    public virtual void SetAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Attack:
                animator.SetTrigger("Attack");
                break;
            case AnimationType.Damage:
                animator.SetTrigger("Damage");
                break;
            case AnimationType.Recovery:
                animator.SetTrigger("Recovery");
                break;
            case AnimationType.Buff:
                animator.SetTrigger("Buff");
                break;
            case AnimationType.Debuff:
                animator.SetTrigger("Debuff");
                break;
            case AnimationType.Death:
                animator.SetTrigger("Death");
                break;
            default:
                Debug.LogWarning($"Unknown animation type: {animationType}");
                break;
        }
    }

    public IEnumerator JumpMotion()
    {
        float bounceHeight = 1.5f;
        float damping = 0.7f;
        float gravity = 1000f;
        float groundY = transform.position.y;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping;  // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);  // 最後に位置を調整
    }
}
