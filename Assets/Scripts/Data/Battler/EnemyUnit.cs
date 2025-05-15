using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUnit : BattleUnit
{
    [SerializeField] public NamePlate namePlate;

    public override void Setup(Battler battler)
    {
        battler.Init();
        base.Setup(battler);
        namePlate.SetName(battler.Base.Name);
    }

    public override void UpdateEnegyUI()
    {
        base.UpdateEnegyUI();
    }
}
