using System;
using UnityEngine;

public class Work
{
    private static IObjectStorage<Work> _workplan;

    private WorkType _workType;
    private Vector2Int _workPos;
    private int _priority;
    private float _maxWorkload;
    private float _workload;
    private IWorkable _assignWorker;

    public WorkType WorkType { get { return _workType; } set { _workType = value; } }
    public Vector2Int WorkPos { get { return _workPos; } set { _workPos = value; } }
    public int Priority { get { return _priority; } set { _priority = value; } }
    public IWorkable AssignWorker { get { return _assignWorker; } set { _assignWorker = value; } }

    public event Action OnStarted;
    public event Action OnProcessed;
    public event Action OnCompleted;

    public static void SetWorkplan(IObjectStorage<Work> workplan)
    {
        _workplan = workplan;
    }

    public void SetNewData(WorkType type, int maxWorkload)
    {
        _workType = type;
        _priority = 3;
        _maxWorkload = maxWorkload;
        _workload = 0;
        _assignWorker = null;
        ResetRegister();
    }

    public void OnProcess()
    {
        // TODO : 작업자의 정보를 받아 작업도를 증가시킴
        OnStarted?.Invoke();

        if (_workload < _maxWorkload)
        {
            OnProcessed?.Invoke();
        }
        else
        {
            OnCompleted?.Invoke();
        }
    }

    public void AddWorkPlan()
    {
        _workplan.AddObject(this);
    }

    public void RegisterOnStarted(Action onStarted)
    {
        OnStarted += onStarted;
    }

    public void RegisterOnProcessed(Action onProcessed)
    {
        OnProcessed += onProcessed;
    }

    public void RegisterOnCompleted(Action onCompleted)
    {
        OnCompleted += onCompleted;
    }

    public void UnRegisterOnStarted(Action onStarted)
    {
        OnStarted -= onStarted;
    }

    public void UnregisterOnProcessed(Action onProcessed)
    {
        OnProcessed -= onProcessed;
    }

    public void UnregisterOnCompleted(Action onCompleted)
    {
        OnCompleted -= onCompleted;
    }

    public void Remove()
    {
        ResetRegister();
        _workplan.RemoveObject(this);
        WorkGenerator.Instance.Remove(this);
    }

    public void ResetRegister()
    {
        OnProcessed = null;
        OnCompleted = null;
    }
}
