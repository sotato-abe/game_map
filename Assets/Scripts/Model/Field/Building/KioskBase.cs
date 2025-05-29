using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building/Kiosk")]
public class KioskBase : BuildingBase
{
    [SerializeField] List<Item> items;

    public List<Item> Items { get => items; }

    public override BuildingType type => BuildingType.Kiosk;
}
