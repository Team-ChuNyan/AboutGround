using System.Collections.Generic;

public class WorkPlan : IObjectStorage<Work>
{
    private WorkDictionary _workDictionary;
    private HashSet<IWorkable> _waitWorker;

    public WorkPlan()
    {
        _workDictionary = new();
        _waitWorker = new();
    }

    public void AddObject(Work item)
    {
        _workDictionary.Add(item);
        AssignWorkToWaitWorker();
    }

    public void AddWaitWorker(IWorkable worker)
    {
        _waitWorker.Add(worker);
        AssignWork(worker);
    }

    private void AssignWorkToWaitWorker()
    {
        foreach (var worker in _waitWorker)
        {
            AssignWork(worker);
        }
    }

    private void AssignWork(IWorkable worker)
    {
        if (_workDictionary.TryGetWork(WorkType.Carry, out Work work) == false)
            return;

        work.IsAssignWorker = true;
        worker.Work(work);
    }

    public void RemoveObject(Work work)
    {
        _workDictionary.Remove(work);
    }
}
