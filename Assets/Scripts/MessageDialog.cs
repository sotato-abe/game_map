using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDialog : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject messagePanel;
    [SerializeField] GameObject commandPanel;
    [SerializeField] GameObject itemPanel;
    [SerializeField] Image dialogBackground;

    BattleDialogType battleDialogType;

    public void changeDialogType(BattleAction action)
    {
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

    private void SetTalkPanel()
    {
        Debug.Log("SetTalkPanel");
        itemPanel.SetActive(false);
        commandPanel.SetActive(false);
        messagePanel.SetActive(true);
    }

    private void SetAttackPanel()
    {
        Debug.Log("SetAttackPanel");
        commandPanel.SetActive(false);
        itemPanel.SetActive(false);
        messagePanel.SetActive(true);
    }

    private void SetCommandPanel()
    {
        Debug.Log("SetCommandPanel");
        messagePanel.SetActive(false);
        itemPanel.SetActive(false);
        commandPanel.SetActive(true);
    }

    private void SetItemPanel()
    {
        Debug.Log("SetItemPanel");
        messagePanel.SetActive(false);
        commandPanel.SetActive(false);
        itemPanel.SetActive(true);
    }

    private void SetRunDialog()
    {
        Debug.Log("SetRunDialog");
        commandPanel.SetActive(false);
        itemPanel.SetActive(false);
        messagePanel.SetActive(true);
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
