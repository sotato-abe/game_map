using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerBattler : Battler
{
    PropertyPanel propertyPanel;

    public override void Init()
    {
        propertyPanel = GameObject.FindObjectOfType<PropertyPanel>();

        base.Init();
        UpdatePropertyPanel();
    }

    public void UpdatePropertyPanel()
    {
        propertyPanel.Init(Money, Disk, Key);
    }
}
