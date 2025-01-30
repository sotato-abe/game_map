using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasRenderer))]
public class Card : MonoBehaviour
{
     private Image cardImage;  // 表示用のTextMeshProUGUIフィールド
     private float radius = 5f; // 角の半径
     private Vector2 size = new Vector2(200f, 300f); // カードのサイズ
     private Color cardColor = Color.white; // カードの色

    private Texture2D cardTexture; // カードの背景テクスチャ
    private Material material;

    public void SetCharacter(Battler battler)
    {
        cardImage.sprite = battler.Base.Sprite;
    }
}
