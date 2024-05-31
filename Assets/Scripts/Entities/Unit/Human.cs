public class Human : Unit, IWorkable, IAttackable
{
    public HumanLocalStatus HumanLocalStatus;
    public WorkerHandler _workProcess;
    private IInventory _inven;

    public WorkerHandler WorkerHandler { get { return _workProcess; } }
    public bool IsWorking { get { return HumanLocalStatus.IsWorking; } set { HumanLocalStatus.IsWorking = value; } }

    public WorkType WorkTypeFlag { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private const int DEFAULT_HUMAN_INVENTORY_SLOT = 4;

    public override void Awake()
    {
        base.Awake();
        UniversalStatus.Race = RaceType.Human;
        HumanLocalStatus = new HumanLocalStatus();
        _inven = new SlotInventory(DEFAULT_HUMAN_INVENTORY_SLOT);
        _workProcess = gameObject.AddComponent<WorkerHandler>();
    }

    public void StartWork()
    {
        HumanLocalStatus.IsWorking = true;
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
        HumanLocalStatus.IsWorking = false;
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
