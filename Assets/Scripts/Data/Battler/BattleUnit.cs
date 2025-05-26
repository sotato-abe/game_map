using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void SetTalkMessage(string message, PanelType panelType = PanelType.Default)
    {
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, panelType, message);
        blowing.gameObject.SetActive(true);
        blowing.AddMessageList(talkMessage);
    }

    public void SetMessage(TalkMessage talkMessage)
    {
        blowing.gameObject.SetActive(true);
        blowing.AddMessageList(talkMessage);
    }

    public void SetBattlerTalkMessage(MessageType messageType)
    {
        // 指定された messageType に一致するメッセージをすべて取得
        var matchingMessages = Battler.Base.MessageList
            .Where(m => m.messageType == messageType)
            .ToList();

        // ランダムに1つ選ぶ（見つからない場合は null）
        TalkMessage foundMessage = matchingMessages.Count > 0
            ? matchingMessages[UnityEngine.Random.Range(0, matchingMessages.Count)]
            : null;

        // 見つかったメッセージを使うか、デフォルトメッセージを使う
        string battlerMessage = foundMessage != null
            ? foundMessage.message
            : messageType.GetDefaultMessage();

        PanelType panelType = foundMessage != null
            ? foundMessage.panelType
            : PanelType.Default;

        SetTalkMessage(battlerMessage, panelType);
    }

    public void TakeAttack(Attack attack)
    {
        SetMotion(MotionType.Shake);
        SetBattlerTalkMessage(MessageType.Damage);
        Battler.TakeAttack(attack);
        UpdateEnegyUI();
        UpdateEnchantUI();
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
        Battler.TakeEnchant(enchantList);
        EncahntMessage(enchantList);
        UpdateEnchantUI();
    }

    private void EncahntMessage(List<Enchant> enchantList)
    {
        int buffCount = 0;
        foreach (Enchant enchant in enchantList)
        {
            EnchantData enchantData = EnchantDatabase.Instance?.GetData(enchant.Type);
            if (enchantData.buffType == BuffType.Buff)
            {
                buffCount++;
            }
            else if (enchantData.buffType == BuffType.Debuff)
            {
                buffCount--;
            }
        }
        if (buffCount > 0)
        {
            SetBattlerTalkMessage(MessageType.Recovery);
        }
        else if (buffCount < 0)
        {
            SetBattlerTalkMessage(MessageType.Damage);
        }
        else
        {
            SetTalkMessage("。。。");
        }
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
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();
            enchantUnit.SetEnchant(enchant);
        }
    }

    public virtual void SetStatusDialog()
    {
        statusDialog.Setup(Battler);
    }

    public void SetMotion(MotionType motion)
    {
        characterCard.SetCardMotion(motion);
    }
}
