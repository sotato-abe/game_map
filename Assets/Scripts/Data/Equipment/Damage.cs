using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Damage
{
    public SkillType type;
    public int val;

    public SkillType Type { get => type; }
    public int Val { get => val; }

}
