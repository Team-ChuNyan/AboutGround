using System;
using System.Collections.Generic;

public class WorkProcess
{
    private readonly List<Work> _works;
    private WorkType _workType;
    private bool _isStarting;
    private bool _isCanceling;
    private int _priority;

    private event Action<IWorkable> Finished;

    public List<Work> Works { get { return _works; } }
    public WorkType WorkType { get { return _workType; } }
    public bool IsStarting { get { return _isStarting; } set { _isStarting = value; } }
    public bool IsCanceling { get { return _isCanceling; } set { _isCanceling = value; } }
    public int Priority { get { return _priority; } }

    public int Count { get { return _works.Count; } }

    public WorkProcess()
    {
        _works = new();
    }

    public void SetNewWorks(WorkType type)
    {
        _works.Clear();
        _workType = type;
        _isStarting = false;
        _isCanceling = false;
        _priority = 3;
        Finished = null;
    }

    public void AddWorkLoad(int index, float value)
    {
        var work = _works[index];
        work.Workload += value;

        _works[index] = work;
    }

    public void Add(Work work)
    {
        _works.Add(work);
    }

    public void OnFinished(IWorkable worker)
    {
        Finished?.Invoke(worker);
    }

    public void RegisterFinished(Action<IWorkable> action)
    {
        Finished += action;
    }

    public void Clear()
    {
        _works.Clear();
    }
}
