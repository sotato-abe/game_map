using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{

    public Battler Battler { get; set; }
    [SerializeField] TalkPanel talkPanel;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
    }

    public enum Motion
    {
        Jump,
        Attack,
    }

    Motion motion;

    public IEnumerator SetTalkMessage(string message)
    {
        if (talkPanel != null) // Nullチェックを追加
        {
            talkPanel.gameObject.SetActive(true);
            yield return talkPanel.TypeDialog(message);
        }
        else
        {
            Debug.LogError("talkPanel is not assigned!");
        }
    }

    public virtual void UpdateUI()
    {

    }

    public void SetMotion(Motion targetMotion)
    {
        motion = targetMotion;
        switch (motion)
        {
            case Motion.Jump:
                StartCoroutine(JumpMotion());
                break;
            case Motion.Attack:
                break;
        }
    }

    private IEnumerator JumpMotion()
    {
        float bounceHeight = 40f;
        float damping = 0.5f;
        float gravity = 5000f;
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
