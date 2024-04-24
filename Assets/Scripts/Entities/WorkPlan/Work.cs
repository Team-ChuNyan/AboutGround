using System;
using UnityEngine;

public struct Work
{
    private Vector2Int _workPos;
    private float _maxWorkload;
    private float _workload;

    private Action<IWorkable> _started;
    private Action<IWorkable> _processed;
    private Action<IWorkable> _completed;

    public readonly Vector2Int WorkPos { get { return _workPos; } }
    public readonly float MaxWorkload { get { return _maxWorkload; } }
    public float Workload 
    {
        readonly get { return _workload; }
        set
        {
            _workload = value < _maxWorkload ? value : _maxWorkload;
        } 
    }

    public readonly bool IsComplete => _workload >= _maxWorkload;

    public Work(int maxWorkload, Vector2Int pos)
    {
        _maxWorkload = maxWorkload;
        _workload = 0;
        _workPos = pos;

        _started = null;
        _processed = null;
        _completed = null;
    }

    public readonly void OnStarted(IWorkable worker)
    {
        _started?.Invoke(worker);
    }

    public readonly void OnProcessed(IWorkable worker)
    {
        _processed?.Invoke(worker);
    }
    public readonly void OnCompleted(IWorkable worker)
    {
        _completed?.Invoke(worker);
    }

    #region Register
    public void RegisterStarted(Action<IWorkable> action)
    {
        _started += action;
    }

    public void RegisterProcessed(Action<IWorkable> action)
    {
        _processed += action;
    }

    public void RegisterCompleted(Action<IWorkable> action)
    {
        _completed += action;
    }

    public void UnregisterStarted(Action<IWorkable> action)
    {
        _started -= action;
    }

    public void UnregisterProcessed(Action<IWorkable> action)
    {
        _processed -= action;
    }

    public void UnregisterCompleted(Action<IWorkable> action)
    {
        _completed -= action;
    }
    #endregion
}
