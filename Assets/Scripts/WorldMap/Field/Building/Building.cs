using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingType Type = BuildingType.Building;

    public void Setup(BuildingType type)
    {
        Type = type;
    }
}
