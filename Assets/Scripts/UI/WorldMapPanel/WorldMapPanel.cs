using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : MonoBehaviour
{
    [SerializeField] WorldMapCameraManager worldMapCameraManager;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] FieldPlayer fieldPlayer;

    private bool isCameraUpFlg = false;
    private bool isCameraBottomFlg = false;
    private bool isCameraRightFlg = false;
    private bool isCameraLeftFlg = false;

    public bool isActive = false; // フラグを追加

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
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive; // フラグを設定
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
    }
    public void ChangeActive()
    {
        isActive = !isActive; // フラグをトグル
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
    }

    public void ChangeActiveFromField()
    {
        ChangeActive();
        if (isActive)
        {
            ageTimePanel.SetTimeSpeed(TimeState.Live); // 時間を止める
        }
        else
        {
            ageTimePanel.SetTimeSpeed(TimeState.Fast); // 時間を進める
        }
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
