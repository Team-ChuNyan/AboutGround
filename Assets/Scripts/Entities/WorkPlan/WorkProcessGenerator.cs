using System;
using System.Collections.Generic;

public class WorkProcessGenerator : Singleton<WorkProcessGenerator>
{
    private Queue<WorkProcess> _inactiveWork;
    private WorkProcess _newWorkProcess;

    private event Action<WorkProcess> GeneratedWork;
    private event Action<WorkProcess> RemovedWork;

    public WorkProcessGenerator() 
    {
        _inactiveWork = new();
    }

    public void RegisterGenerated(Action<WorkProcess> action)
    {
        GeneratedWork += action;
    }

    public void RegisterRemoved(Action<WorkProcess> action)
    {
        RemovedWork += action;
    }

    public WorkProcessGenerator SetNewWork(WorkType type)
    {
        if (_inactiveWork.TryDequeue(out _newWorkProcess) == false)
        {
            _newWorkProcess = new();
        }

        _newWorkProcess.SetNewWorks(type);
        return this;
    }

    public WorkProcessGenerator AddWork(Work work)
    {
        _newWorkProcess.Add(work);
        return this;
    }

    public WorkProcessGenerator RegisterFinished(Action<IWorkable> action)
    {
        _newWorkProcess.RegisterFinished(action);
        return this;
    }

    public WorkProcess Generate()
    {
        GeneratedWork?.Invoke(_newWorkProcess);
        return _newWorkProcess;
    }

    public void Remove(WorkProcess work)
    {
        RemovedWork?.Invoke(work);
        _inactiveWork.Enqueue(work);
    }
}