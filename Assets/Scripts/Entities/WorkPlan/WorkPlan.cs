using System.Collections.Generic;

public class WorkPlan
{
    private WorkDictionary _workDictionary;
    private List<IWorkable> _waitWorker;
    private List<IWorkable> _workWorker;

    public WorkPlan()
    {
        _workDictionary = new();
        _waitWorker = new();
        _workWorker = new();
        WorkGenerator.Instance.RegisterGenerated(AddWork);
        WorkGenerator.Instance.RegisterRemoved(Remove);
    }

    public void AddWork(WorkProcess work)
    {
        _workDictionary.Add(work);
        AssignWorkToWaitWorker();
    }

    public void AddWaitWorker(IWorkable worker)
    {
        if (_waitWorker.Contains(worker) == true)
            return;

        _waitWorker.Add(worker);
        AssignWork(worker);
    }

    private void AssignWorkToWaitWorker()
    {
        for (int i = 0; i < _waitWorker.Count; i++)
        {
            AssignWork(_waitWorker[i]);
        }
    }

    private void AssignWork(IWorkable worker)
    {
        if (worker.IsWorking == true)
            return;

        if (_workDictionary.TryGetWork(WorkType.Carry, out WorkProcess work) == false)
            return;

        work.IsStarting = true;
        _waitWorker.Remove(worker);
        _workWorker.Add(worker);
        work.RegisterFinished(ReRegisterWaiting);
        worker.ContractWork(work);
        worker.StartWork();
    }

    public void Remove(WorkProcess work)
    {
        _workDictionary.Remove(work);
    }

    public void Remove(IWorkable worker)
    {
        if (_waitWorker.Remove(worker) == false)
        {
            _workWorker.Remove(worker);
        }
    }

    private void ReRegisterWaiting(IWorkable worker)
    {
        _workWorker.Remove(worker);
        AddWaitWorker(worker);
    }
}
