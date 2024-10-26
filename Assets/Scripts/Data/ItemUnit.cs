using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour
{
    public Item Item { get; set; }
    [SerializeField] Image image;


    public virtual void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
    }
}
