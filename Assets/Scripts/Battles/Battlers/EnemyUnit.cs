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
    public override void Setup(Battler battler)
    {
        base.Setup(battler);

        image.sprite = battler.Base.Sprite;
        nameText.SetText(battler.Base.Name);
        attackText.SetText($"{battler.Base.Attack}");
    }
}
