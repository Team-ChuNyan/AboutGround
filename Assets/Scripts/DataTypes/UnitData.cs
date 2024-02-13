public struct UnitData
{
    private RaceType _race;
    private string _name;
    private string _description;
    private float _moveSpeed; // 공통적인 부분은 스크립터블로 수정

    public readonly RaceType Race { get { return _race; } }
    public readonly string Name { get { return _name; } }
    public readonly string Description { get { return _description; } }
    public readonly float MoveSpeed { get { return _moveSpeed; } }


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
}