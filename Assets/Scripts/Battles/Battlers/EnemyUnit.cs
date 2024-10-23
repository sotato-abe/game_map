using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUnit : BattleUnit
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    public override void Setup(Battler battler)
    {
        base.Setup(battler);

        image.sprite = battler.Base.Sprite;
        nameText.SetText(battler.Base.Name);
        attackText.SetText($"{battler.Base.Attack}");
        techniqueText.SetText($"{battler.Base.Technique}");
        defenseText.SetText($"{battler.Base.Defense}");
        speedText.SetText($"{battler.Base.Speed}");
    }
}
