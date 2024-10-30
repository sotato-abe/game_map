using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : MonoBehaviour
{
    public Command Command { get; set; }
    [SerializeField] Image image;

    public virtual void Setup(Command command)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
    }
}
