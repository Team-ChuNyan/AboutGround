using System;
using UnityEngine;

public class BuildingUniversalStatus
{
    public BuildingType BuildingType;
    public BuildingCategory Category;

    [NonSerialized] public Mesh Mesh;
    [NonSerialized] public Material Material;
}
