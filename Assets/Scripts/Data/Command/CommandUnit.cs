using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : Unit
{
    public Command Command { get; set; }
    [SerializeField] Image unitImage;
    [SerializeField] UnitStatusLayer unitStatusLayer;
    [SerializeField] CommandDialog dialog;


    public virtual void Setup(Command command)
    {
        Command = command;
        unitImage.sprite = Command.Base.Sprite;
        dialog.gameObject.SetActive(true);
        dialog.Setup(Command);
        SetStatus(UnitStatus.Active);
    }

    public void OnPointerEnter()
    {
        dialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        dialog.ShowDialog(false);
        StartCoroutine(OnPointer(false));
    }

    public void SetStatus(UnitStatus status)
    {
        unitStatusLayer.Setup(status);
    }
}
