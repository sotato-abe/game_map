using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBoard : MonoBehaviour
{
    [SerializeField] AttackPanel attackPanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] public PouchPanel pouchPanel;
    [SerializeField] public BagPanel bagPanel;
    [SerializeField] public DeckPanel deckPanel;
    ActionType action;

    public void Init()
    {
        action = 0;
    }

    public void ChangeActionPanel(ActionType targetAction)
    {
        action = targetAction;
        ResetPanel();
        switch (action)
        {
            case ActionType.Talk:
                SetTalkPanel();
                break;
            case ActionType.Attack:
                SetAttackPanel();
                break;
            case ActionType.Command:
                SetCommandPanel();
                break;
            case ActionType.Pouch:
                SetPouchPanel();
                break;
            case ActionType.Bag:
                SetBagPanel();
                break;
            case ActionType.Deck:
                SetDeckPanel();
                break;
            case ActionType.Escape:
                SetEscapeDialog();
                break;
        }
    }

    // public void TargetSelection(bool targetDirection)
    // {
    //     switch (action)
    //     {
    //         case ActionType.Talk:
    //             break;
    //         case ActionType.Attack:
    //             break;
    //         case ActionType.Command:
    //             break;
    //         case ActionType.Pouch:
    //             pouchPanel.SelectItem(targetDirection);
    //             break;
    //         case ActionType.Bag:
    //             bagPanel.SelectDialog(targetDirection);
    //             break;
    //         case ActionType.Deck:
    //             break;
    //         case ActionType.Escape:
    //             break;
    //     }
    // }

    private void ResetPanel()
    {
        attackPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        pouchPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        deckPanel.gameObject.SetActive(false);
    }

    private void SetTalkPanel()
    {
    }

    private void SetAttackPanel()
    {
        attackPanel.gameObject.SetActive(true);
        attackPanel.PanelOpen();
    }

    private void SetCommandPanel()
    {
        commandPanel.gameObject.SetActive(true);
        commandPanel.PanelOpen();
    }

    private void SetPouchPanel()
    {
        pouchPanel.gameObject.SetActive(true);
        pouchPanel.PanelOpen();
    }

    private void SetBagPanel()
    {
        bagPanel.gameObject.SetActive(true);
        bagPanel.PanelOpen();
    }

    private void SetDeckPanel()
    {
        deckPanel.gameObject.SetActive(true);
        deckPanel.PanelOpen();
    }

    private void SetEscapeDialog()
    {
    }

    public void CloseActionPanel()
    {
        ResetPanel();
    }
}
