using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{

    public Battler Battler { get; set; }

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
    }

    public IEnumerator OpenEnemyDialog()
    {
        Debug.Log("OpenEnemyDialog!!");
        float initialBounceHeight = 40f;  // 初めのバウンドの高さ
        float dampingFactor = 0.5f;      // 減衰率（バウンドの大きさがどれくらいずつ減るか）
        float gravity = 5000f;            // 重力の強さ
        float groundY = transform.position.y;  // 地面のY座標（開始位置に基づく）
        float currentBounceHeight = initialBounceHeight;
        float verticalVelocity = Mathf.Sqrt(2 * gravity * currentBounceHeight);
        bool isFalling = true;

        while (currentBounceHeight >= 0.1f)  // バウンドが小さくなって停止するまでループ
        {
            // バウンド中の垂直方向の動き
            if (isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;  // 重力で速度を減少させる
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;  // 垂直方向に移動

                // 地面に到達したらバウンド
                if (transform.position.y <= groundY)
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
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                // 上昇が終わったら落下に切り替える
                if (verticalVelocity <= 0)
                {
                    isFalling = true;
                }
            }

            yield return null;  // 次のフレームまで待つ
        }

        // 最終的な位置を調整（小さなバウンドを終えた後に地面に戻す）
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
    }
}
