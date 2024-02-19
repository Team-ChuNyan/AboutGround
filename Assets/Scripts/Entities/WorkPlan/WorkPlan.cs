using System.Collections.Generic;

public class WorkPlan
{
    private WorkDictionary _workDictionary;
    private List<IWorkable> _waitWorker;

    public WorkPlan()
    {
        _workDictionary = new WorkDictionary();
    }

    public void AddWork(Work work)
    {
        _workDictionary.Add(work);
    }

    public void AddWaitWorker(IWorkable worker)
    {
        _waitWorker.Add(worker);
    }

    public void RemoveWork(Work work) 
    {
        _workDictionary.Remove(work);
    }
}
