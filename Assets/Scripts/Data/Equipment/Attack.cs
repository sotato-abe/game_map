using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Attack
{
    List<Enegy> DamageList;
    List<Enegy> RecoveryList;
    List<Enchant> EnchantList;
    TargetType Target = TargetType.Enemy;
}

