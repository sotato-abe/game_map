using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerBattler : Battler
{
    PropertyPanel propertyPanel;

    public int SkillPoint { get; set; } = 0;

    public override void Init()
    {
        propertyPanel = GameObject.FindObjectOfType<PropertyPanel>();

        base.Init();
        UpdatePropertyPanel();
    }

    public void UpdatePropertyPanel()
    {
        propertyPanel.Init(Money, Disk, Key);
    }

    public void AcquisitionExp(int exp)
    {
        Exp += exp;
        if (Exp >= 100)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Exp -= 100;
        Level++;
        SkillPoint++;
    }

    public void EnegyUp(EnegyType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case EnegyType.Life:
                    MaxLife += 10;
                    break;
                case EnegyType.Battery:
                    MaxBattery += 5;
                    break;
            }
            SkillPoint -= 1;
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
    }

    public void StatusUp(StatusType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case StatusType.ATK:
                    Attack.val += 1;
                    break;
                case StatusType.TEC:
                    Technique.val += 1;
                    break;
                case StatusType.DEF:
                    Defense.val += 1;
                    break;
                case StatusType.SPD:
                    Speed.val += 1;
                    break;
                case StatusType.LUK:
                    Luck.val += 1;
                    break;
                case StatusType.MMR:
                    Memory.val += 1;
                    break;
                case StatusType.STG:
                    Storage.val += 1;
                    break;
                case StatusType.POC:
                    Pouch.val += 1;
                    break;
                case StatusType.BAG:
                    Bag.val += 1;
                    break;

            }
            SkillPoint -= 1;
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
    }
}
