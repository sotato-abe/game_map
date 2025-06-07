using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{
    [SerializeField] public NamePlate namePlate;
    [SerializeField] public GameObject levelUpPlate;
    [SerializeField] EnegyBar lifeEnegyBar;
    [SerializeField] EnegyBar batteryEnegyBar;
    [SerializeField] EnegyBar soulEnegyBar;

    public void Setup(PlayerBattler battler)
    {
        base.Setup((Battler)battler);
        namePlate.minWidth = 80; // 最小幅を設定
        namePlate.SetName(battler.Base.Name);
        CheckSkillPoint();
    }

    public override void SetEnegy()
    {
        base.SetEnegy();
        lifeEnegyBar.SetEnegy(EnegyType.Life, Battler.ColLife, Battler.Life);
        batteryEnegyBar.SetEnegy(EnegyType.Battery, Battler.ColBattery, Battler.Battery);
        soulEnegyBar.SetEnegy(EnegyType.Soul, 100, Battler.Soul);
        CheckSkillPoint();
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
        lifeEnegyBar.ChangeEnegyVal(Battler.Life);
        batteryEnegyBar.ChangeEnegyVal(Battler.Battery);
        soulEnegyBar.ChangeEnegyVal(Battler.Soul);
        CheckSkillPoint();
    }

    public override void SetStatusDialog()
    {
        base.SetStatusDialog();
        CheckSkillPoint();
    }

    public void CheckSkillPoint()
    {
        if (Battler is PlayerBattler playerBattler)
        {
            if (playerBattler.SkillPoint > 0)
                levelUpPlate.SetActive(true);
            else
                levelUpPlate.SetActive(false);
        }
    }
}
