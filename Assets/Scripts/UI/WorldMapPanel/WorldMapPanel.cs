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

    public bool isActive = false; // フラグを追加

    void Update()
    {
        if (!isActive) return; // フラグがfalseの場合は処理をスキップ
        
        if (isCameraUpFlg || Input.GetKey(KeyCode.UpArrow))
        {
            worldMapCameraManager.UpTarget(); // 上に移動
        }
        if (isCameraBottomFlg || Input.GetKey(KeyCode.DownArrow))
        {
            worldMapCameraManager.DownTarget(); // 下に移動
        }
        if (Input.inputString.Contains("@"))
        {
            OnCurrentPosition(); // 下に移動
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

    public void OnCurrentPosition()
    {
        worldMapCameraManager.ResetCamera(); // カメラの位置をリセット
    }
}
