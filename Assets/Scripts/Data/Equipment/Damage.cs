using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Damage
{
    public AttackType AttackType { get; private set; } // 攻撃の種類
    public EnegyType EnegyType { get; private set; } // スキルの種類
    public int Val { get; set; } // ダメージの値

    public Damage(AttackType type, EnegyType enegyType, int value)
    {
        AttackType = type;
        EnegyType = enegyType;
        Val = value;
    }
}

