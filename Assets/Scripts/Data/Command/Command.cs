using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Command
{
    [SerializeField] CommandBase _base;

    public CommandBase Base { get => _base; }

    public void Init()
    {
    }
}
