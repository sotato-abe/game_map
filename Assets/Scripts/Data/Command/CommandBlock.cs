using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBlock : Block
{
    public Command command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image maskImage;
    // [SerializeField] CommandDialog commandDialog;

    public delegate void DeleteCommandDelegate(Command block);
    public event DeleteCommandDelegate OnDeleteCommand;

    public delegate void SellCommandDelegate(Command block);
    public event SellCommandDelegate OnSellCommand;

    public void Setup(Command command)
    {
        this.command = command;
        image.sprite = command.Base.Sprite;
        // commandDialog.Setup(command);
    }

    public void OnPointerEnter()
    {
        if (command != null)
        {
            // commandDialog.ShowDialog(true);
            StartCoroutine(OnPointer(true));
        }
    }

    public void OnPointerExit()
    {
        if (command != null)
        {
            // commandDialog.ShowDialog(false);
            StartCoroutine(OnPointer(false));
        }
    }

    public void RemoveCommand()
    {
        OnDeleteCommand?.Invoke(command);
    }

    public void SellCommand()
    {
        OnSellCommand?.Invoke(command);
    }
}
