public class UnitLocalStatus
{
    public PropOwner Owner;
    public string Name;
    public string Description;
    public float Weight;
 
    public SelectPropType SelectPropType { get { return Owner == PropOwner.Player ? SelectPropType.PlayerUnit : SelectPropType.NPC; } }
}
