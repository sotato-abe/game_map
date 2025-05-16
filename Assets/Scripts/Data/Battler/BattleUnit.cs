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
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] GameObject enchantList;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
        Battler.Init();
        characterCard.SetCharacter(battler);
        statusDialog.Setup(Battler);
        SetEnegy();
        UpdateEnchantUI();
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

    public virtual void UpdateEnegyUI()
    {
        lifeBar.ChangeEnegyVal(Battler.Life);
        batteryBar.ChangeEnegyVal(Battler.Battery);
        soulBar.ChangeEnegyVal(Battler.Soul);
    }

    public void TakeEnchant(List<Enchant> enchantList)
    {
        SetMotion(MotionType.Shake);
        SetBattlerTalkMessage(MessageType.Damage);
        Battler.TakeEnchant(enchantList);
        UpdateEnchantUI();
    }

    public void DecreaseEnchant()
    {
        Battler.DecreaseEnchant();
        UpdateEnchantUI();
    }

    private void UpdateEnchantUI()
    {
        List<Enchant> enchants = Battler.Enchants;
        // enchantList内を初期化
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }
        // enchantList内にスキルを追加
        foreach (Enchant enchant in enchants)
        {
            Debug.Log("Enchant: " + enchant.Type);
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();
            enchantUnit.SetEnchant(enchant);
        }
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
