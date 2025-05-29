using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TalkMessage
{
    public MessageType messageType;
    public PanelType panelType = PanelType.Default;
    public string message;

    public TalkMessage(MessageType messageType, PanelType panelType, string message)
    {
        this.messageType = messageType;
        this.panelType = panelType;
        this.message = message;
    }
}
