using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleUnit : MonoBehaviour
{

    public Battler Battler { get; set; }
    [SerializeField] CharacterCard characterCard;
    [SerializeField] BattlerEnegyIcon lifeIcon;
    [SerializeField] BattlerEnegyIcon batteryIcon;
    [SerializeField] BattlerEnegyIcon soulIcon;
    [SerializeField] BattlerStatusDialog statusDialog;
    [SerializeField] TalkPanel talkPanel;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
        characterCard.SetCharacter(battler);
        UpdateEnegyUI();
        statusDialog.Setup(Battler);
    }

    public IEnumerator SetTalkMessage(string message)
    {
        if (talkPanel != null) // Nullチェックを追加
        {
            talkPanel.gameObject.SetActive(true);
            yield return talkPanel.TypeDialog(message);
        }
        else
        {
            Debug.LogError("talkPanel is not assigned!");
        }
    }

    public virtual void UpdateEnegyUI()
    {
        Enegy life = new Enegy(EnegyType.Life, Battler.Life);
        Enegy battery = new Enegy(EnegyType.Battery, Battler.Battery);
        Enegy soul = new Enegy(EnegyType.Soul, Battler.Soul);

        lifeIcon.SetEnegy(life);
        batteryIcon.SetEnegy(battery);
        soulIcon.SetEnegy(soul);
        statusDialog.Setup(Battler);
    }

    public void SetMessage(MessageType messageType)
    {
        List<TalkMessage> messagesList = Battler.Base.Messages;

        // messagesListが空なら処理を中断
        if (messagesList == null || messagesList.Count == 0)
        {
            Debug.LogWarning("SetMessage: メッセージリストが空です");
            return;
        }

        // Indexが範囲外にならないように修正
        int index = Mathf.Min(2, messagesList.Count - 1);
        StartCoroutine(SetTalkMessage(messagesList[index].message)); // ← message をプロパティに修正
    }

    public void SetMotion(MotionType motion)
    {
        characterCard.SetCardMotion(motion);
    }

    public void OnPointerEnter()
    {
        StartCoroutine(statusDialog.ShowDialog(true));
    }

    public void OnPointerExit()
    {
        StartCoroutine(statusDialog.ShowDialog(false));
    }
}
