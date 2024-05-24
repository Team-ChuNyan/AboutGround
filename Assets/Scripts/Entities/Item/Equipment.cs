public class Equipment : Item, IEquipable
{
    private EquipmentStatus _equipmentData;

    public EquipmentStatus EquipmentData { get { return _equipmentData; } private set { _equipmentData = value; } }

    public void SetEquipmentData(EquipmentStatus data)
    {
        EquipmentData = data;
    }

    public void Equip()
    {
    }
}
