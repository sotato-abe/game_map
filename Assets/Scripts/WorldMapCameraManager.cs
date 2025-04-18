using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCameraManager : MonoBehaviour
{

    private int worldWidth = 40; // ワールドマップの幅
    private int worldHeight = 100; // ワールドマップの高さ

    private Vector3Int currentPos; // 現在のカメラ位置

    // WorldMapのPlayer位置にカメラを合わせる
    public void TargetPlayer(Vector3Int targetPos)
    {
        currentPos = targetPos; // 現在のカメラ位置を更新
        // カメラの位置をターゲットの位置に合わせる
        Vector3 cameraPos = new Vector3(targetPos.x + 0.5f, targetPos.y, -10); // Z軸は-10に固定
        transform.position = cameraPos;
    }

    public void ResetCamera()
    {
        TargetPlayer(currentPos); // 現在のカメラ位置に戻す
    }

    public void UpTarget()
    {
        // カメラの位置を上に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y += 0.05f; // 上に1ユニット移動
        if (cameraPos.y > worldHeight) // 上限を超えないように制限
        {
            cameraPos.y = 0;
        }
        transform.position = cameraPos;
    }

    public void DownTarget()
    {
        // カメラの位置を下に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y -= 0.05f; // 下に1ユニット移動
        if (cameraPos.y < 0) // 下限を超えないように制限
        {
            cameraPos.y = worldHeight;
        }
        transform.position = cameraPos;
    }

    public void LeftTarget()
    {
        // カメラの位置を左に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x -= 0.05f; // 左に1ユニット移動
        if (cameraPos.x < 0) // 左限を超えないように制限
        {
            cameraPos.x = worldWidth;
        }
        transform.position = cameraPos;
    }

    public void RightTarget()
    {
        // カメラの位置を右に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x += 0.05f; // 右に1ユニット移動
        if (cameraPos.x > worldWidth) // 右限を超えないように制限
        {
            cameraPos.x = 0;
        }
        transform.position = cameraPos;
    }
}