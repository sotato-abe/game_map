using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : MonoBehaviour
{
    public Command Command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image dialogBackground;

    public virtual void Setup(Command command)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
    }

    public virtual void Targetfoucs(bool focusFlg)
    {
        float alpha = focusFlg ? 1.0f : 0.0f;
        Color bgColor = dialogBackground.color;
        bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
        dialogBackground.color = bgColor;
    }
}
