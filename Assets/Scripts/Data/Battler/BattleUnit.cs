using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleUnit : MonoBehaviour
{

    public Battler Battler { get; set; }
    [SerializeField] CharacterCard characterCard;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI batteryText;
    [SerializeField] TextMeshProUGUI soulText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TalkPanel talkPanel;

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
        characterCard.SetCharacter(battler);
        lifeText.SetText($"{battler.Life}");
        batteryText.SetText($"{battler.Battery}");
        soulText.SetText($"{battler.Soul}");
        attackText.SetText($"{battler.Base.Attack}");
        techniqueText.SetText($"{battler.Base.Technique}");
        defenseText.SetText($"{battler.Base.Defense}");
        speedText.SetText($"{battler.Base.Speed}");
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

    public virtual void UpdateUI()
    {
        lifeText.SetText($"{Battler.Life}");
        batteryText.SetText($"{Battler.Battery}");
        soulText.SetText($"{Battler.Soul}");
        attackText.SetText($"{Battler.Base.Attack}");
        techniqueText.SetText($"{Battler.Base.Technique}");
        defenseText.SetText($"{Battler.Base.Defense}");
        speedText.SetText($"{Battler.Base.Speed}");
    }

    public void SetMotion(MotionType motion)
    {
        characterCard.SetCardMotion(motion);
    }
}
