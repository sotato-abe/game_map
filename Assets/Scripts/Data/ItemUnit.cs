using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image dialogBackground;

    public virtual void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
    }

    public virtual void Targetfoucs(bool focusFlg)
    {
        float alpha = focusFlg ? 1.0f : 0.0f;
        Color bgColor = dialogBackground.color;
        bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
        dialogBackground.color = bgColor;
    }
}
