using System;

public class Work
{
    private WorkType _workType;
    private int _priority;
    private float _maxWorkload;
    private float _workload;
    private bool _isAssignWorker;

    public WorkType WorkType { get { return _workType; } }
    public int Priority { get { return _priority; } }
    public bool IsAssignWorker { get { return _isAssignWorker; } }

    public event Action OnStarted;
    public event Action OnProcessed;
    public event Action OnCompleted;

    public Work SetWorkData(WorkType type, int maxWorkload)
    {
        _workType = type;
        _maxWorkload = maxWorkload;
        _workload = 0;

        return this;
    }

    public Work SetPriority(int value = 3)
    {
        _priority = value;

        return this;
    }

    public Work SetAssignWorker(bool isAssign)
    {
        _isAssignWorker = isAssign;

        return this;
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

    public void RegisterProcessed(Action onProcessed)
    {
        OnProcessed += onProcessed;
    }

    public void RegisterCompleted(Action onCompleted)
    {
        OnCompleted += onCompleted;
    }

    public void UnregisterProcessed(Action onProcessed)
    {
        OnProcessed -= onProcessed;
    }

    public void UnregisterCompleted(Action onCompleted)
    {
        OnCompleted -= onCompleted;
    }

    public void ResetRegister()
    {
        OnProcessed = null;
        OnCompleted = null;
    }
}
