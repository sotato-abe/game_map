using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSlot : Unit
{
    public Command command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image maskImage;
    [SerializeField] CommandDialog commandDialog;

    public void Setup(Command command)
    {
        this.command = command;
        image.sprite = command.Base.Sprite;
        commandDialog.Setup(command);
    }

    public void OnPointerEnter()
    {
        if (command != null)
        {
            commandDialog.ShowDialog(true);
            StartCoroutine(OnPointer(true));
        }
    }

    public void OnPointerExit()
    {
        if (command != null)
        {
            commandDialog.ShowDialog(false);
            StartCoroutine(OnPointer(false));
        }
    }

    public void RemoveCommand()
    {
        this.command = null;
        maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, 0f);
        image.sprite = null;
    }
}
