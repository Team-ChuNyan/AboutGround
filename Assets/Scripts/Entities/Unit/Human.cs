public class Human : Unit, IWorkable, IAttackable
{
    public WorkerHandler _workProcess;
    private IInventory _inven;
    private bool _isWorking;

    public WorkerHandler WorkerHandler { get { return _workProcess; } }
    public bool IsWorking { get { return _isWorking; } set { _isWorking = value; } }

    private const int DEFAULT_HUMAN_INVENTORY_SLOT = 4;

    public override void Awake()
    {
        base.Awake();
        UnitData.SetRace(RaceType.Human);
        _inven = new SlotInventory(DEFAULT_HUMAN_INVENTORY_SLOT);
        _workProcess = gameObject.AddComponent<WorkerHandler>();
    }

    public void StartWork()
    {
        _isWorking = true;
        _workProcess.StartProcess(this);
    }

    public bool IsPossibleToWork(WorkType type)
    {
        // TODO :  해당 유형의 작업이 가능한지
        return true;
    }

    public IInventory GetInventory()
    {
        return _inven;
    }

    public void CompleteWork()
    {
        _isWorking = false;
    }

    public void PutDownItem(IPackable pack, int amount)
    {
        _inven.PutDownItem(pack, transform.position, amount);
    }

    public void StopWork()
    {
        StopMovement();
        _workProcess.StopProcess();
    }

    public void ContractWork(WorkProcess work)
    {
        _workProcess.ContractWork(work);
    }

    public void AddWorkload(float value)
    {
        _workProcess.AddWorkload(value);
    }
}
