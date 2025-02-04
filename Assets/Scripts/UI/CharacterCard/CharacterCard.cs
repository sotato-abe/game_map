using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasRenderer))]
public class CharacterCard : MonoBehaviour
{
    [SerializeField] BattlerBase testbattler;
    [SerializeField] private TextMeshProUGUI nameText;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private Image cardImage;  // 表示用のTextMeshProUGUIフィールド

    public void SetCharacter(Battler battler)
    {
        cardImage.sprite = battler.Base.Sprite;
        nameText.SetText(battler.Base.Name);
    }
}
