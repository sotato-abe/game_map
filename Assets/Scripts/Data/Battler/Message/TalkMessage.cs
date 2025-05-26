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
}
