using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] new string name;
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] Sprite sprite;

    public string Name { get => name; }
    public int MaxHP { get => maxHP; }
    public int Attack { get => attack; }
    public Sprite Sprite { get => sprite; }
}
