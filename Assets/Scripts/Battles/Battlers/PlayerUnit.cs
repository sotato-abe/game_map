using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : BattleUnit
{
    public void Setup(PlayerBattler battler)
    {
        base.Setup((Battler)battler);
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
    }
}
