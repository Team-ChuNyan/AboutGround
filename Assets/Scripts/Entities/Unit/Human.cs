public class Human : Unit, IWorkable, IAttackable
{
    private IItemStorage _inven;
    private const int DEFAULT_HUMAN_INVENTORY_SLOT = 4;

    public override void Awake()
    {
        base.Awake();
        UnitData.SetRace(RaceType.Human);
        _inven = new SlotInventory(DEFAULT_HUMAN_INVENTORY_SLOT);
    }

    public void Work(Work work)
    {
        // TODO : 일하는 기능
        Move(work.WorkPos);
        OnArrived += work.OnProcess;

    }

    public bool IsPossibleToWork()
    {
        // TODO :  작업 가능 여부를 체크
        return true;
    }

    public bool IsPossibleToWork(WorkType type)
    {
        // TODO :  해당 유형의 작업이 가능한지
        return true;
    }

    public IItemStorage GetInventory()
    {
        return _inven;
    }
}
