public class ItemUniversalStatus
{
    private ItemType _type;
    private GradeType _gradeType;
    private string _name;
    private string _description;
    private float _weight;
    private float _maxDurability;
    private bool _isStack;
    private int _maxAmount;

    public ItemType Type { get { return _type; } set { _type = value; } }
    public GradeType GradeType { get { return _gradeType; } set { _gradeType = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public float Weight { get { return _weight; } set { _weight = value; } }
    public float MaxDurability { get { return _maxDurability; } set { _maxDurability = value; } }
    public bool IsStacked { get { return _isStack; } set { _isStack = value; } }
    public int MaxAmount { get { return _maxAmount; } set { _maxAmount = value; } }
}
