using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ability")]
public class AbilityBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] RarityType rarity;
    [SerializeField] Sprite sprite;
    [SerializeField, TextArea] string description;

    public string Name { get => name; }
    public RarityType Rarity { get => rarity; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
}
