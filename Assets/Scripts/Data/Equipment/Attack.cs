using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Attack
{
    public TargetType Target { get; set; } = TargetType.Enemy;
    public List<Enegy> DamageList { get; set; } = new List<Enegy>();
    public List<Enegy> RecoveryList { get; set; } = new List<Enegy>();
    public List<Enchant> EnchantList { get; set; } = new List<Enchant>();

    public Attack(TargetType target, List<Enegy> damageList, List<Enegy> recoveryList, List<Enchant> enchantList)
    {
        Target = target;
        DamageList = damageList;
        RecoveryList = recoveryList;
        EnchantList = enchantList;
    }
}

