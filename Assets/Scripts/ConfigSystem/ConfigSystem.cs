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
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] TitlePanel titlePanel;

    public bool isActive = true; // フラグを追加

    void Update()
    {
        if (!isActive) return; // フラグがfalseの場合は処理をスキップ

        if (Input.GetKeyDown(KeyCode.M)) // Mキーでワールドマップを表示
        {
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
        titlePanel.SetTitle(TitleType.Config); // タイトルパネルを設定

        worldMapPanel.SetActive(true); // ワールドマップパネルを表示する
        messagePanel.SetActive(false);
        fieldInfoPanel.SetActive(false);
        OnConfigOpen?.Invoke(); // リザーブイベントを発火
    }

    public void CloseConfig()
    {
        worldMapPanel.SetActive(false);
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 3)
            {
                OnConfigClose?.Invoke(); // エンカウントイベントを発火
            }
        }
        titlePanel.SetActive(false, CheckAllComplete);
        messagePanel.SetActive(true, CheckAllComplete);
        fieldInfoPanel.SetActive(true, CheckAllComplete);
    }
}
