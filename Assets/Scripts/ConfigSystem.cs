using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

// JSONのマップデータを読み込みフィールドデータを返す
public class ConfigSystem : MonoBehaviour
{
    public UnityAction OnConfigOpen; // リザーブイベント
    public UnityAction OnConfigClose; // エンカウントイベント
    [SerializeField] WorldMapPanel worldMapPanel;

    public bool isActive = true; // フラグを追加

    void Update()
    {
        if (!isActive) return; // フラグがfalseの場合は処理をスキップ

        if (Input.GetKeyDown(KeyCode.M)) // Mキーでワールドマップを表示
        {
            Debug.Log("Mキーが押されました。");
            if (worldMapPanel.isActive)
            {
                CloseConfig(); // ワールドマップを非表示にする
            }
            else
            {
                OpenConfig(); // ワールドマップを表示する
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConfig(); // ワールドマップを非表示にする
        }
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive; // フラグを設定
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を変更
    }

    public void OpenConfig()
    {
        worldMapPanel.SetActive(true); // ワールドマップパネルを表示する
        OnConfigOpen?.Invoke(); // リザーブイベントを発火
    }

    public void CloseConfig()
    {
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示にする
        OnConfigClose?.Invoke(); // エンカウントイベントを発火
    }
}
