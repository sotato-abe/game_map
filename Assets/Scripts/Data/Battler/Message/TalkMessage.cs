using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class TalkMessage : ScriptableObject
{
    [SerializeField] new string message;
    [SerializeField] MessageType messageType;
    [SerializeField] PanelType panelType;
}
