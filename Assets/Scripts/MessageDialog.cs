using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDialog : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] MessagePanel messagePanel;
    [SerializeField] CommandPanel commandPanel;
    [SerializeField] ItemPanel itemPanel;
    [SerializeField] Image dialogBackground;
    BattleAction battleAction;
    public ItemPanel ItemPanel => itemPanel;

    public void Init()
    {
        battleAction = 0;
    }

    public void changeDialogType(BattleAction action)
    {
        battleAction = action;
        switch (action)
        {
            case BattleAction.Talk:
                SetTalkPanel();
                break;
            case BattleAction.Attack:
                SetAttackPanel();
                break;
            case BattleAction.Command:
                SetCommandPanel();
                break;
            case BattleAction.Item:
                SetItemPanel();
                break;
            case BattleAction.Run:
                SetRunDialog();
                break;
        }
    }

    public void TargetSelection(bool targetDirection)
    {
        switch (battleAction)
        {
            case BattleAction.Talk:
                break;
            case BattleAction.Attack:
                break;
            case BattleAction.Command:
                commandPanel.SelectCommand(targetDirection);
                break;
            case BattleAction.Item:
                itemPanel.SelectItem(targetDirection);
                break;
            case BattleAction.Run:
                break;
        }
    }

    private void SetTalkPanel()
    {
        Debug.Log("SetTalkPanel");
        itemPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(true);
    }

    private void SetAttackPanel()
    {
        Debug.Log("SetAttackPanel");
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(true);
    }

    private void SetCommandPanel()
    {
        Debug.Log("SetCommandPanel");
        messagePanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(true);
    }

    private void SetItemPanel()
    {
        Debug.Log("SetItemPanel");
        messagePanel.gameObject.SetActive(false);
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(true);
    }

    private void SetRunDialog()
    {
        Debug.Log("SetRunDialog");
        commandPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(true);
    }

    // 現在使用していない
    public IEnumerator SetTransparency(float alpha)
    {
        // 背景のImageコンポーネントの透明度を変更
        if (dialogBackground != null)
        {
            Color bgColor = dialogBackground.color;
            bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
            dialogBackground.color = bgColor;
        }
        yield return null;
    }

    public IEnumerator SetMessageText(string message)
    {
        yield return StartCoroutine(messagePanel.GetComponent<MessagePanel>().TypeDialog(message));
    }
}
