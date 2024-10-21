using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{

    [SerializeField] Image image;
    [SerializeField] new TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI mp;
    public override void Setup(Battler battler)
    {
        base.Setup(battler);
        image.sprite = battler.Base.Sprite;
        name.SetText(battler.Base.Name);
        level.SetText($"Lv : {battler.Level}");
        hp.SetText($"HP : {battler.HP} / {battler.MaxHP}");
        mp.SetText($"MP : {battler.MP} / {battler.MaxMP}");
    }
}
