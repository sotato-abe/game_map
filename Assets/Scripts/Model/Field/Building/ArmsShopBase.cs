using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building/ArmsShop")]
public class ArmsShopBase : BuildingBase
{
    [SerializeField] List<Equipment> equipments;

    public List<Equipment> Equipments { get => equipments; }
    public override BuildingType type => BuildingType.ArmsShop;
}
