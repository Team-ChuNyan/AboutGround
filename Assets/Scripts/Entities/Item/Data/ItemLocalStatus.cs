
public class ItemLocalStatus
{
    public bool IsPack;
    public bool CanAccess;
    public int Amount;
    public float Durability;
    public bool IsActivity;

    public void Init()
    {
        IsPack = false;
        CanAccess = false;
        IsActivity = false;
    }
}
