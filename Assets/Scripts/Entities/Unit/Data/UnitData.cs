public struct UnitData
{
    private RaceType _race;
    public PropOwner Owner;
    private string _name;
    private string _description;
    private float _moveSpeed; // 공통적인 부분은 스크립터블로 수정
    private float _weight;
    private float _maxWeight;

    public readonly RaceType Race { get { return _race; } }
    public readonly string Name { get { return _name; } }
    public readonly string Description { get { return _description; } }
    public readonly float MoveSpeed { get { return _moveSpeed; } }
    public float Weight { get { return _weight; } set { _weight = value; } }
    public float MaxWeight { get { return _maxWeight; } set { _maxWeight = value; } }

    public readonly SelectPropType SelectPropType { get { return Owner == PropOwner.Player ? SelectPropType.PlayerUnit : SelectPropType.NPC; } }

    public UnitData SetRace(RaceType race)
    {
        _race = race;
        return this;
    }

    public UnitData SetName(string name)
    {
        _name = name;
        return this;
    }

    public UnitData SetDesc(string desc)
    {
        _description = desc;
        return this;
    }

    public UnitData SetMoveSpeed(float value)
    {
        _moveSpeed = value;
        return this;
    }

    public UnitData SetOwner(PropOwner owner)
    {
        Owner = owner;
        return this;
    }
}