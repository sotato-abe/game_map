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

    public virtual void UpdateEnegyUI()
    {
        lifeBar.ChangeEnegyVal(Battler.Life);
        batteryBar.ChangeEnegyVal(Battler.Battery);
        soulBar.ChangeEnegyVal(Battler.Soul);
        statusDialog.Setup(Battler);
    }

    public void SetMotion(MotionType motion)
    {
        characterCard.SetCardMotion(motion);
    }
}
