using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Battler/BattlerGroup")]
public class BattlerGroup : ScriptableObject
{
    [SerializeField] List<Battler> BattlerList;

    public List<Battler> GetBattlerList()
    {
        return BattlerList;
    }

    public List<Battler> GetRandomBattlerList()
    {
        List<Battler> copy = new List<Battler>(BattlerList);

        if (copy.Count == 0)
            return new List<Battler>();

        int count = Random.Range(1, copy.Count + 1);

        // シャッフル
        for (int i = copy.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (copy[i], copy[j]) = (copy[j], copy[i]); // C# 7.0 以降のタプルスワップ
        }

        return copy.Take(count).ToList();
    }
}
