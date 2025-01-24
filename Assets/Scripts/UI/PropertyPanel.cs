using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PropertyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyField;  // お金表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI diskField;  // メモリ表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI keyField;  // 鍵表示用のTextMeshProUGUIフィールド

    // 初期化して現在の時間を設定
    public void Init(int money, int disk, int key)
    {
        SetMoney(money);
        SetDisk(disk);
        SetKey(key);
    }

    public void SetMoney(int money)
    {
        moneyField.text = $"{money}";
    }

    public void SetDisk(int disk)
    {
        diskField.text = $"{disk}";
    }

    public void SetKey(int key)
    {
        keyField.text = $"{key}";
    }
}
