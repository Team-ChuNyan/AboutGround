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
    private bool _isAssignWorker;

    public WorkType WorkType { get { return _workType; } set { _workType = value; } }
    public Vector2Int WorkPos { get { return _workPos; } set { _workPos = value; } }
    public int Priority { get { return _priority; } set { _priority = value; } }
    public bool IsAssignWorker { get { return _isAssignWorker; } set { _isAssignWorker = value; } }

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
        _isAssignWorker = false;
        ResetRegister();
    }

    public void OnProcess(IWorkable worker)
    {
        // TODO : 작업자의 정보를 받아 작업도를 증가시킴
        if (_workload < _maxWorkload)
        {
            OnProcessed?.Invoke();
        }
        else
        {
            OnCompleted?.Invoke();
        }
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
