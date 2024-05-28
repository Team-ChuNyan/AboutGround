using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUniversalStatus
{
    public List<RequestItem> RequestItem;
    public BuildingType BuildingType;
    public BuildingCategory Category;

    [NonSerialized] public Mesh Mesh;
    [NonSerialized] public Material Material;
}
