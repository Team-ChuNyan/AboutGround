public class Equipment : Item, IEquipable
{
    private EquipmentData _equipmentData;

    public EquipmentData EquipmentData { get { return _equipmentData; } set { _equipmentData = value; } }

    public void Equip()
    {
    }
}
