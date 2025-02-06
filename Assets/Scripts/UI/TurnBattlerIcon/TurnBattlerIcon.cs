using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnBattlerIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;  // 表示用のTextMeshProUGUIフィールド
    private bool isActive = true;
    public void SetCharacter(Sprite characterSprite)
    {
        iconImage.sprite = characterSprite;
    }

    public void SetActive(bool isActiveFlag)
    {
        isActive = isActiveFlag;
    }
}

