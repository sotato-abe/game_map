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
    [SerializeField] FieldCharacterSystem fieldCharacterSystem;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
        Battler.Init();
        characterCard.SetCharacter(battler);
        statusDialog.Setup(Battler);
        SetEnegy();
        UpdateEnchantUI();
    }

    public void SetFieldCharacterSystem(FieldCharacterSystem fieldCharacterSystem)
    {
        this.fieldCharacterSystem = fieldCharacterSystem;
    }

    public virtual void SetEnegy()
    {
        lifeBar.SetEnegy(EnegyType.Life, Battler.ColLife, Battler.Life);
        batteryBar.SetEnegy(EnegyType.Battery, Battler.ColBattery, Battler.Battery);
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
        StartCoroutine(blowing.AddMessage(talkMessage));
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

    public void ExecuteAttack()
    {
        // 攻撃時のモーションを設定
        fieldCharacterSystem.SetCharacterMotion(Battler, AnimationType.Attack);
    }

    public void TakeAttack(Attack attack)
    {
        SetBattlerReaction(attack);
        Battler.TakeAttack(attack);
        UpdateEnegyUI();
        UpdateEnchantUI();
        if (Battler.Life <= 0)
        {
            fieldCharacterSystem.SetCharacterMotion(Battler, AnimationType.Death);
            SetBattlerTalkMessage(MessageType.Lose);
        }
    }

    public virtual void UpdateEnegyUI()
    {
        lifeBar.ChangeEnegyVal(Battler.Life);
        batteryBar.ChangeEnegyVal(Battler.Battery);
        soulBar.ChangeEnegyVal(Battler.Soul);
    }

    private void SetBattlerReaction(Attack attack)
    {
        var reactions = new List<(int count, MotionType motion, AnimationType animationType, MessageType message, bool isEnchant)>
        {
            (attack.DamageList.Count, MotionType.Shake, AnimationType.Damage, MessageType.Damage, false),
            (attack.RecoveryList.Count, MotionType.Shake, AnimationType.Recovery, MessageType.Recovery, false),
            (attack.EnchantList.Count, MotionType.Shake, AnimationType.Buff, MessageType.Question, true)
        }
        ;

        var maxReaction = reactions.OrderByDescending(r => r.count).First();

        if (!maxReaction.isEnchant)
        {
            SetMotion(maxReaction.motion);
            fieldCharacterSystem.SetCharacterMotion(Battler, maxReaction.animationType);
            SetBattlerTalkMessage(maxReaction.message);
        }
        else
        {
            int buffCount = 0;
            foreach (var enchant in attack.EnchantList)
            {
                var data = EnchantDatabase.Instance?.GetData(enchant.Type);
                if (data == null) continue;

                buffCount += data.buffType == BuffType.Buff ? 1 :
                             data.buffType == BuffType.Debuff ? -1 : 0;
            }

            if (buffCount == 0)
            {
                SetMotion(MotionType.Move);
                fieldCharacterSystem.SetCharacterMotion(Battler, AnimationType.Buff);
                SetBattlerTalkMessage(MessageType.Question);
            }
            else if (buffCount > 0)
            {
                SetMotion(MotionType.Randam);
                fieldCharacterSystem.SetCharacterMotion(Battler, AnimationType.Debuff);
                SetBattlerTalkMessage(MessageType.Recovery);
            }
            else
            {
                SetMotion(MotionType.Shake);
                fieldCharacterSystem.SetCharacterMotion(Battler, AnimationType.Damage);
                SetBattlerTalkMessage(MessageType.Damage);
            }
        }
    }

    public void DecreaseEnchant()
    {
        Battler.DecreaseEnchant();
        UpdateEnchantUI();
    }

    public void ClearEnchant()
    {
        Battler.ClearEnchant();
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
            EnchantIcon enchantIcon = enchantObject.GetComponent<EnchantIcon>();
            enchantIcon.SetEnchant(enchant);
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
