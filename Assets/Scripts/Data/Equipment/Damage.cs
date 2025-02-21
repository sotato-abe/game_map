using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Damage
{
    public SkillType Type { get; private set; } // スキルの種類
    public int Val { get; set; } // ダメージの値

    public Damage(SkillType type, int value)
    {
        Type = type;
        Val = value;
    }
}

