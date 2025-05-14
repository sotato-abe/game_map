using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{
    [SerializeField] public NamePlate namePlate;

    public void Setup(PlayerBattler battler)
    {
        base.Setup((Battler)battler);
        namePlate.SetName(battler.Base.Name);
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
    }
}
