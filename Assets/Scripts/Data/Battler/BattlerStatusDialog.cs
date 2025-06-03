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
    [SerializeField] TextMeshProUGUI luckText;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Battler battler)
    {
        attackText.SetText(battler.ColPower.val.ToString());
        techniqueText.SetText(battler.ColTechnique.val.ToString());
        defenseText.SetText(battler.ColDefense.val.ToString());
        speedText.SetText(battler.ColSpeed.val.ToString());
        luckText.SetText(battler.ColLuck.val.ToString());
    }
}
