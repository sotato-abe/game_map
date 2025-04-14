using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldInfoIcon : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetIconMotion(MotionType targetMotion)
    {
        switch (targetMotion)
        {
            case MotionType.Jump:
                StartCoroutine(JumpMotion());
                break;
            case MotionType.Shake:
                StartCoroutine(ShakeMotion());
                break;
        }
    }

    private IEnumerator JumpMotion()
    {
        float bounceHeight = 30f;
        float damping = 0.7f;
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

    private IEnumerator ShakeMotion()
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.6f; // シェイク時間
        float shakeMagnitude = 50f; // シェイクの振れ幅
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude) * 0.1f;
            transform.position = originalPosition + new Vector3(xOffset, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // 元の位置に戻す
    }
}
