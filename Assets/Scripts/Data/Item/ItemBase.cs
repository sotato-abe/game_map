using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int life;
    [SerializeField] int battery;
    [SerializeField] Sprite sprite;
    [SerializeField] string description;

    public string Name { get => name; }
    public int Life { get => life; }
    public int Battery { get => battery; }
    public Sprite Sprite { get => sprite; }
    public string Description { get => description; }
}
