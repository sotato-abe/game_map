using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnBattlerIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;  // 表示用のTextMeshProUGUIフィールド
    public void SetCharacter(Sprite characterSprite)
    {
        iconImage.sprite = characterSprite;
    }
}

