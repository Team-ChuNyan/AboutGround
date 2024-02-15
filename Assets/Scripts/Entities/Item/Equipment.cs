public class Equipment : Item, IEquipable
{
    private EquipmentData _equipmentData;

    public EquipmentData EquipmentData { get { return _equipmentData; } private set { _equipmentData = value; } }

    public void SetEquipmentData(EquipmentData data)
    {
        EquipmentData = data;
    }

    public void Equip()
    {
    }
}
