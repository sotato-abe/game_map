using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ability")]
public class Ability : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] new string description;

    public string Name { get => name; }
    public string Description { get => description; }
}
