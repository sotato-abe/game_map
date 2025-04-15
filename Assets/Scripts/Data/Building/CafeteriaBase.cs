using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building/Cafeteria")]
public class CafeteriaBase : BuildingBase
{
    [SerializeField] List<Item> items;

    public List<Item> Items { get => items; }
}
