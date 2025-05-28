using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{
    [SerializeField] public NamePlate namePlate;
    [SerializeField] public GameObject levelUpPlate;

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
        CheckSkillPoint();
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
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
