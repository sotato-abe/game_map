using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Attack
{
    public List<Enegy> DamageList { get; set; } = new List<Enegy>();
    public List<Enegy> RecoveryList { get; set; } = new List<Enegy>();
    public List<Enchant> EnchantList { get; set; } = new List<Enchant>();
    public TargetType Target { get; set; } = TargetType.Enemy;
}

