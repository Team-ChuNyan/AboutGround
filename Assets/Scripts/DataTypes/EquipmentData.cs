using System.Collections.Generic;

public struct EquipmentData
{
    private EquipType _equipType;
    private List<StatusData> _statusData;
    private float _coolTime;

    public EquipType EquipType { get { return _equipType; } set { _equipType = value; } }
    public List<StatusData> GradeType { get { return _statusData; } set { _statusData = value; } }
    public float CoolTime { get { return _coolTime; } set { _coolTime = value; } }
}
