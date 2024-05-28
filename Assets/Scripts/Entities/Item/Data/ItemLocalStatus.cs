
public class ItemLocalStatus
{
    public bool IsPack;
    public bool IsPublicAccess;
    public int Amount;
    public float Durability;
    public bool IsActivity;

    public void Init()
    {
        IsPack = false;
        IsPublicAccess = false;
        IsActivity = false;
    }
}
