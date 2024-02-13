using UnityEngine;

public class BodyPart
{
    private BodyPartType _type;
    private string _name;
    private string _desc;
    private float _maxHp;
    private float _hp;

    public BodyPartType Type { get { return _type; } set { _type = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Desc { get { return _desc; } set { _desc = value; } }
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Hp { get { return _hp; } set { _hp = value; } }

    public BodyPart(BodyPartType type)
    {
        InitializePartType(type);
    }

    public void InitializePartType(BodyPartType type)
    {
        _type = type;
    }

    public float GetEfficiency()
    {
        return Mathf.Round(_hp / _maxHp * 100);
    }
}