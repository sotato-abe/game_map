using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    [SerializeField] BuildingBase _base;

    public BuildingBase Base { get => _base; }
    public string Name { get => _base.Name; }
    public Battler Owner { get => _base.Owner; }
    public string Description { get => _base.Description; }
    public List<Item> Items { get => _base.Items; } // TODO これをItemsにするか？
}
