public class BuildingLocalStatus
{
    public SlotInventory Inventory;
    public BuildingType BuildingType;
    public bool IsBluePrint;

    public BuildingLocalStatus()
    {
        Inventory = new(4);
    }

    public void Init(BuildingUniversalStatus buildingGlobal)
    {
        BuildingType = buildingGlobal.BuildingType;
        IsBluePrint = false;
    }
}
