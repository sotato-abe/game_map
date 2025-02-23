using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu]
public class Skill : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] private List<Enegy> enegys;
    [SerializeField] private List<Enchant> enchants;


    public string Name { get => name; }
    public List<Enegy> Enegys { get => enegys; }
    public List<Enchant> Enchants { get => enchants; }
}
