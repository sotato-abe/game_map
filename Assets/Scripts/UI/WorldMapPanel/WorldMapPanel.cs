using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : MonoBehaviour
{
    private bool isActive = false; // フラグを追加

    public void ChangeActive()
    {
        isActive = !isActive; // フラグをトグル
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
    }
}
