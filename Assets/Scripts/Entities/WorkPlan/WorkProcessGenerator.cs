using System;
using System.Collections.Generic;

public class WorkProcessGenerator : Singleton<WorkProcessGenerator>, IObjectGenerator<WorkProcess>
{
    private readonly Queue<WorkProcess> _inactiveWork;
    private WorkProcess _newWorkProcess;

    private event Action<WorkProcess> Generated;
    private event Action<WorkProcess> Destroyed;

    public WorkProcessGenerator()
    {
        _inactiveWork = new();
    }

    public WorkProcessGenerator SetNewWork(WorkType type)
    {
        _newWorkProcess = GetNewWorkProcess();
        _newWorkProcess.SetNewWorks(type);
        return this;
    }

    private WorkProcess GetNewWorkProcess()
    {
        if (_inactiveWork.TryDequeue(out var workProcess) == false)
        {
            workProcess = new();
        }
        return workProcess;
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
        Generated?.Invoke(_newWorkProcess);
        return _newWorkProcess;
    }

    public void OnDestroyed(WorkProcess work)
    {
        Destroyed?.Invoke(work);
        _inactiveWork.Enqueue(work);
    }

    #region Register
    public void RegisterGenerated(Action<WorkProcess> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<WorkProcess> action)
    {
        Destroyed += action;
    }
    #endregion
}
