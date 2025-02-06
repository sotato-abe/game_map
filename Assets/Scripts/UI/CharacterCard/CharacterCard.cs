using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasRenderer))]
public class CharacterCard : MonoBehaviour
{
    [SerializeField] BattlerBase testbattler;
    [SerializeField] private TextMeshProUGUI nameText;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private Image cardImage;  // 表示用のTextMeshProUGUIフィールド

    private void OnEnable()
    {
        StartCoroutine(MoveMotion());
    }

    public void SetCharacter(Battler battler)
    {
        cardImage.sprite = battler.Base.Sprite;
        nameText.SetText(battler.Base.Name);
    }

    public void SetCardMotion(MotionType targetMotion)
    {
        switch (targetMotion)
        {
            case MotionType.Move:
                StartCoroutine(MoveMotion());
                break;
            case MotionType.Jump:
                StartCoroutine(JumpMotion());
                break;
            case MotionType.Shake:
                StartCoroutine(ShakeMotion());
                break;
            case MotionType.Attack:
                break;
            case MotionType.Randam:
                StartCoroutine(RandamMotion());
                break;
            case MotionType.Rotate:
                StartCoroutine(RotateMotion());
                break;
        }
    }

    private IEnumerator MoveMotion()
    {
        Vector3 originalPosition = transform.position;
        float moveRange = 10f;  // 移動範囲
        float moveSpeed = 2f;  // 移動スピード

        Vector3 targetPosition = originalPosition;

        while (true)
        {
            // 一定間隔でターゲットを更新
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = originalPosition + new Vector3(
                    Random.Range(-moveRange, moveRange),
                    Random.Range(-moveRange, moveRange),
                    0
                );
            }

            // ターゲットに向かってスムーズに移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null; // 毎フレーム実行
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

    private IEnumerator ShakeMotion()
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.3f; // シェイク時間
        float shakeMagnitude = 40f; // シェイクの振れ幅
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

    private IEnumerator RandamMotion()
    {
        Vector3 originalPosition = transform.position;
        float duration = 1.5f; // モーションの合計時間
        float moveSpeed = 5f;  // 移動スピード
        float moveRange = 10f; // 移動範囲
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ランダムな方向へのターゲット位置を設定
            Vector3 randomOffset = new Vector3(
                Random.Range(-moveRange, moveRange),
                Random.Range(-moveRange, moveRange),
                0
            );

            Vector3 targetPosition = originalPosition + randomOffset;
            float moveTime = Random.Range(0.3f, 0.7f); // 移動する時間をランダム化
            float moveElapsed = 0f;

            // ゆっくり移動
            while (moveElapsed < moveTime)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, (moveElapsed / moveTime));
                moveElapsed += Time.deltaTime * moveSpeed;
                yield return null;
            }

            elapsed += moveTime;
        }

        // 最後に元の位置へ戻す
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.position = originalPosition; // 最終的に完全に元の位置へ
    }

    // TODO：未完成：回転しているように見えない
    private IEnumerator RotateMotion()
    {
        float rotateAngle = 180f; // 1回の回転角度（180度）
        int totalRotations = 4; // 回転回数（4回転）
        float duration = 3f; // 3秒で回転を終わらせる（回転速度に影響）

        Quaternion startRotation = transform.rotation;

        // 目標角度（回転回数 x 1回転の角度）
        Quaternion targetRotation = Quaternion.Euler(0, rotateAngle * totalRotations, 0);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0から1までの補間値を計算
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // 最終的にターゲット角度に合わせる
    }

    private IEnumerator SwingMotion()
    {
        float rotateAngle = 30f; // 30度回転（右奥・左手前）
        float rotateSpeed = 3f; // 回転速度
        float duration = 0.3f; // 1回の回転時間
        float elapsed = 0f;

        for (int i = 0; i < 2; i++) // 2回揺れる
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, rotateAngle, 0);

            elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                elapsed += Time.deltaTime * rotateSpeed;
                yield return null;
            }

            transform.rotation = targetRotation;

            // 逆方向に揺れる
            startRotation = transform.rotation;
            targetRotation = Quaternion.Euler(0, -rotateAngle, 0);

            elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                elapsed += Time.deltaTime * rotateSpeed;
                yield return null;
            }

            transform.rotation = targetRotation;
        }

        // 最後に元の角度に戻す
        Quaternion resetRotation = Quaternion.Euler(0, 0, 0);
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.rotation = Quaternion.Lerp(transform.rotation, resetRotation, t);
            elapsed += Time.deltaTime * rotateSpeed;
            yield return null;
        }

        transform.rotation = resetRotation;
    }
}
