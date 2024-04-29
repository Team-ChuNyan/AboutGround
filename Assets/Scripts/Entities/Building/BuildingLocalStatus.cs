public struct BuildingLocalStatus
{
    public BuildingType BuildingType;
    public bool IsBluePrint;

    public BuildingLocalStatus(BuildingUniversalStatus buildingGlobal)
    {
        BuildingType = buildingGlobal.BuildingType;
        IsBluePrint = false;
    }
}
