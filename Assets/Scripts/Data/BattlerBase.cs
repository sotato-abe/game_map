using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] int maxHP;
    [SerializeField] int maxMP;
    [SerializeField] int attack;
    [SerializeField] Sprite sprite;

    public string Name { get => name; }
    public int MaxHP { get => maxHP; }
    public int MaxMP { get => maxMP; }
    public int Attack { get => attack; }
    public Sprite Sprite { get => sprite; }
}
