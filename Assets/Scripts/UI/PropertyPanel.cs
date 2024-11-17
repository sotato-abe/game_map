using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PropertyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyField;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI diskField;  // 表示用のTextMeshProUGUIフィールド

    // 初期化して現在の時間を設定
    public void Init(int money,int disk)
    {
        moneyField.text = $"{money}";
        diskField.text = $"{disk}";
    }

    public void SetMoney(int money)
    {
        moneyField.text = $"{money}";
    }

    public void SetDisk(int disk)
    {
        diskField.text = $"{disk}";
    }
}
