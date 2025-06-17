using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemCategoryToggle : ToggleButton
{
    [SerializeField] SlidePanel itemPanel;
    [SerializeField] SlidePanel commandPanel;

    protected override void OnActivated()
    {
        itemPanel.SetActive(true);
        commandPanel.SetActive(false);
    }

    protected override void OnDeactivated()
    {
        itemPanel.SetActive(false);
        commandPanel.SetActive(true);
    }
}