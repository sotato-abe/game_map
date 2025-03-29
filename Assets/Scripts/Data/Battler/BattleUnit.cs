using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleUnit : MonoBehaviour
{

    public Battler Battler { get; set; }
    [SerializeField] CharacterCard characterCard;
    [SerializeField] BattlerEnegyBar lifeBar;
    [SerializeField] BattlerEnegyBar batteryBar;
    [SerializeField] BattlerEnegyBar soulBar;
    [SerializeField] BattlerStatusDialog statusDialog;
    [SerializeField] Blowing blowing;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
        characterCard.SetCharacter(battler);
        statusDialog.Setup(Battler);
        SetEnegy();
    }

    private void SetEnegy()
    {
        lifeBar.SetEnegy(EnegyType.Life, Battler.MaxLife, Battler.Life);
        batteryBar.SetEnegy(EnegyType.Battery, Battler.MaxBattery, Battler.Battery);
        soulBar.SetEnegy(EnegyType.Soul, 100, Battler.Soul);
    }

    public void OnPointerEnter()
    {
        statusDialog.ShowDialog(true);
    }

    public void OnPointerExit()
    {
        statusDialog.ShowDialog(false);
    }

    public IEnumerator SetTalkMessage(string message)
    {
        if (blowing != null) // Nullチェックを追加
        {
            blowing.gameObject.SetActive(true);
            yield return blowing.TypeDialog(message);
        }
        else
        {
            Debug.LogError("blowing is not assigned!");
        }
    }

    public virtual void UpdateEnegyUI()
    {
        lifeBar.ChangeEnegyVal(Battler.Life);
        batteryBar.ChangeEnegyVal(Battler.Battery);
        soulBar.ChangeEnegyVal(Battler.Soul);
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
}
