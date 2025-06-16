using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Field/Building/ArmsShop")]
public class ArmsShopBase : BuildingBase
{
    public override BuildingType type => BuildingType.ArmsShop;
}
