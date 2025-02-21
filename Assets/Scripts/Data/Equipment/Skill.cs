using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu]
public class Skill : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] SkillType type;
    [SerializeField] int val;

    public string Name { get => name; }
    public SkillType Type { get => type; }
    public int Val { get => val; }

}
