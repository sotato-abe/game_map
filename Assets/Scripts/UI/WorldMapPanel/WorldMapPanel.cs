using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : MonoBehaviour
{
    [SerializeField] WorldMapCameraManager worldMapCameraManager;

    private bool isCameraUpFlg = false;
    private bool isCameraBottomFlg = false;
    private bool isCameraRightFlg = false;
    private bool isCameraLeftFlg = false;

    private bool isActive = false; // フラグを追加

    void Update()
    {
        if (!isActive) return; // フラグがfalseの場合は処理をスキップ
        if (isCameraUpFlg)
        {
            worldMapCameraManager.UpTarget(); // 上に移動
        }
        if (isCameraBottomFlg)
        {
            worldMapCameraManager.DownTarget(); // 下に移動
        }
        // if (isCameraRightFlg)
        // {
        //     worldMapCameraManager.RightTarget(); // 右に移動
        // }
        // if (isCameraLeftFlg)
        // {
        //     worldMapCameraManager.LeftTarget(); // 左に移動
        // }
    }

    private void SetActive()
    {
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
    }

    public void ChangeActive()
    {
        isActive = !isActive; // フラグをトグル
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
        SetActive();
    }

    public void OnUpStart()
    {
        isCameraUpFlg = true;
    }

    public void OnUpEnd()
    {
        isCameraUpFlg = false;
    }

    public void OnDownStart()
    {
        isCameraBottomFlg = true;
    }
    public void OnDownEnd()
    {
        isCameraBottomFlg = false;
    }
    public void OnLeftStart()
    {
        isCameraLeftFlg = true;
    }
    public void OnLeftEnd()
    {
        isCameraLeftFlg = false;
    }
    public void OnRightStart()
    {
        isCameraRightFlg = true;
    }
    public void OnRightEnd()
    {
        isCameraRightFlg = false;
    }

    public void OnCurrentPosition()
    {
        worldMapCameraManager.ResetCamera(); // カメラの位置をリセット
    }
}
