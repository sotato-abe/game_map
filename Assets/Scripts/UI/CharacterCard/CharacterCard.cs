using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] Image cardImage;

    public virtual void Setup(string name, Sprite image)
    {
        cardImage.sprite = image;
        nameText.SetText(name);
    }
}




