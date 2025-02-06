using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class TalkMessage : ScriptableObject
{
    [SerializeField] public string message;
    [SerializeField] public MessageType messageType;
    [SerializeField] public PanelType panelType;
}
