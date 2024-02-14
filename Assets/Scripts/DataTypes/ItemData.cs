public class ItemData
{
    private ItemType _type;
    private GradeType _gradeType;
    private string _name;
    private string _description;
    private float _maxDurability;
    private float _durability;
    private bool _isStack;
    private int _maxStack;

    public ItemType Type { get { return _type; } }
    public GradeType GradeType { get { return _gradeType; } set { _gradeType = value; } }
    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public float MaxDurability { get { return _maxDurability; } }
    public float Durability { get { return _durability; } }
    public bool IsStacked { get { return _isStack; } }
    public int MaxStack { get { return _maxStack; } }

    public float GetDurabilityPercent()
    {
        return _durability / _maxDurability * 100;
    }
}
