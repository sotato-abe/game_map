using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{
    [SerializeField] CharacterCard characterCard;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI batteryText;
    [SerializeField] TextMeshProUGUI soulText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;

    public override void Setup(Battler battler)
    {
        base.Setup(battler);
        characterCard.SetCharacter(battler);
        lifeText.SetText($"{battler.Life}");
        batteryText.SetText($"{battler.Battery}");
        soulText.SetText($"{battler.Soul}");
        attackText.SetText($"{battler.Base.Attack}");
        techniqueText.SetText($"{battler.Base.Technique}");
        defenseText.SetText($"{battler.Base.Defense}");
        speedText.SetText($"{battler.Base.Speed}");
    }

    public override void UpdateUI()
    {
        lifeText.SetText($"{Battler.Life}");
        batteryText.SetText($"{Battler.Battery}");
        soulText.SetText($"{Battler.Soul}");
        attackText.SetText($"{Battler.Base.Attack}");
        techniqueText.SetText($"{Battler.Base.Technique}");
        defenseText.SetText($"{Battler.Base.Defense}");
        speedText.SetText($"{Battler.Base.Speed}");
    }
}
