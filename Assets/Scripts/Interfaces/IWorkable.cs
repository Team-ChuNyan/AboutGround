public interface IWorkable : IMovable
{
    public WorkerHandler WorkerHandler { get; }
    public bool IsWorking { get; set; }
    public bool IsArrive { get; set; }

    public void StartWork();
    public void ContractWork(WorkProcess work);
    public void StopWork();
    public void CompleteWork();
    public void AddWorkload(float value);
    public void PutDownItem(IPackable pack, int amount);
    public bool IsPossibleToWork(WorkType type);

    public IItemStorage GetInventory();
}
