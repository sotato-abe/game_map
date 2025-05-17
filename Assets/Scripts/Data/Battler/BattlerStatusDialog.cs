using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerStatusDialog : Dialog
{
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Battler battler)
    {
        attackText.SetText(battler.Attack.val.ToString());
        techniqueText.SetText(battler.Technique.val.ToString());
        defenseText.SetText(battler.Defense.val.ToString());
        speedText.SetText(battler.Speed.val.ToString());
    }
}
