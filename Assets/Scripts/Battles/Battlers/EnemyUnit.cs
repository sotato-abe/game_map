using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUnit : BattleUnit
{
    [SerializeField] Image image;
    [SerializeField] new TextMeshProUGUI name;
    public override void Setup(Battler battler)
    {
        base.Setup(battler);
        image.sprite = battler.Base.Sprite;
        name.text = battler.Base.Name;
    }
}
