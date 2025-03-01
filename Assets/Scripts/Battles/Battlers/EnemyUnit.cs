using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUnit : BattleUnit
{
    public override void Setup(Battler battler)
    {
        battler.Init();
        base.Setup(battler);
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
    }
}
