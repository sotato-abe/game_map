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
        Battler.Init();
        characterCard.SetCharacter(battler);
        statusDialog.Setup(Battler);
        SetEnegy();
    }

    public void SetEnegy()
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

    public void SetTalkMessage(string message)
    {
        if (blowing != null) // Nullチェックを追加
        {
            blowing.gameObject.SetActive(true);
            blowing.AddMesageList(message);
        }
        else
        {
            Debug.LogError("blowing is not assigned!");
        }
    }

    public void SetBattlerTalkMessage(MessageType messageType)
    {
        string battlerMessage = Battler.Base.Messages.Find(m => m.messageType == messageType)?.message ?? messageType.GetDefaultMessage();
        SetTalkMessage(battlerMessage);
    }

    public void TakeDamage(List<Damage> damageList)
    {
        SetMotion(MotionType.Shake);
        SetBattlerTalkMessage(MessageType.Damage);
        Battler.TakeDamage(damageList);
        UpdateEnegyUI();
    }

    public void TakeEnchant(List<Enchant> enchantList)
    {
        SetMotion(MotionType.Shake);
        SetBattlerTalkMessage(MessageType.Damage);
        Battler.TakeEnchant(enchantList);
        UpdateEnegyUI();
    }

    public virtual void UpdateEnegyUI()
    {
        lifeBar.ChangeEnegyVal(Battler.Life);
        batteryBar.ChangeEnegyVal(Battler.Battery);
        soulBar.ChangeEnegyVal(Battler.Soul);
        statusDialog.Setup(Battler);
    }

    public void SetStatusDialog()
    {
        statusDialog.Setup(Battler);
    }

    public void SetMotion(MotionType motion)
    {
        characterCard.SetCardMotion(motion);
    }
}
