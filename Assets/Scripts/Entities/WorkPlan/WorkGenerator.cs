using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkGenerator : Singleton<WorkGenerator>
{
    private Queue<Work> _inactiveWork;
    private Work _newWork;

    public WorkGenerator() 
    {
        _inactiveWork = new();
    }

    public void Initialize(WorkPlan workPlan)
    {
        Work.SetWorkplan(workPlan);
    }

    public WorkGenerator CreateNewWork(WorkType type, int maxWorkload)
    {
        if (_inactiveWork.TryDequeue(out _newWork) == false)
        {
            _newWork = new();
        }

        _newWork.SetNewData(type, maxWorkload);
        return this;
    }

    public WorkGenerator SetWorkPos(Vector2Int pos)
    {
        _newWork.WorkPos = pos;

        return this;
    }

    public Work GetWork()
    {
        return _newWork;
    }

    public void Remove(Work work)
    {
        work.Remove();
        _inactiveWork.Enqueue(work);
    }
}
