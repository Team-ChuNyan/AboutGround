using System;
using System.Collections.Generic;

public class WorkDictionary
{
    private MultiKeyDictionary<WorkType, int, List<WorkProcess>> _storage;
    private Dictionary<WorkType, int> _storageCount;

    private const int PRIORITY_COUNT = 5 + 1;

    public List<WorkProcess> this[WorkType type, int priority]
    {
        get { return _storage[type, priority]; }
        set { _storage[type, priority] = value; }
    }

    public WorkDictionary()
    {
        _storage = new();
        _storageCount = new();

        int enumCount = Enum.GetValues(typeof(WorkType)).Length;
        for (int i = 0; i < enumCount; i++)
        {
            WorkType type = (WorkType)i;
            _storage[type] = new();
            _storageCount.Add(type, 0);
            for (int j = 0; j < PRIORITY_COUNT; j++)
            {
                _storage.Add((WorkType)i, j, new (8));
            }
        }
    }

    public void Add(WorkProcess work)
    {
        WorkType type = work.WorkType;
        int priority = work.Priority;

        _storage[type][priority].Add(work);
        _storageCount[type]++;
    }

    public void Remove(WorkProcess work)
    {
        WorkType type = work.WorkType;
        int priority = work.Priority;

        _storage[type][priority].Remove(work);
        _storageCount[type]--;
    }

    public void ChangePriority(WorkProcess work, int priority)
    {
        WorkType type = work.WorkType;
        int beforePriority = work.Priority;

        _storage[type][beforePriority].Remove(work);
        _storage[type][priority].Add(work);
    }

    public bool TryGetWork(WorkType type, out WorkProcess work)
    {
        work = null;

        if (_storageCount[type] > 0)
        {
            for (int i = 0; i < PRIORITY_COUNT; i++)
            {
                if (_storage[type, i].Count == 0)
                    continue;

                var workList = _storage[type, i];
                for (int j = 0; j < workList.Count; j++)
                {
                    if (workList[j].IsStarting == true)
                        continue;

                    work = workList[j];
                    return true;
                }
            }
        }

        return false;
    }
}
