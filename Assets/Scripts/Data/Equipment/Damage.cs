using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Damage
{
    public AttackType AttackType { get; private set; } // 攻撃の種類
    // TODO 名前が気持ち悪いから変えたい
    public int SubType { get; private set; } // スキルの種類
    public int Val { get; set; } // ダメージの値

    public Damage(AttackType type, int subType, int value)
    {
        AttackType = type;
        SubType = subType;
        Val = value;
    }
}

