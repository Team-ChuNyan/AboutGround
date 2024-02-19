using System.Collections.Generic;

public class WorkDictionary
{
    private MultiKeyDictionary<WorkType, int, List<Work>> _storage;

    private const int PRIORITY_COUNT = 5 + 1;

    public List<Work> this[WorkType type, int priority]
    {
        get { return _storage[type, priority]; }
        set { _storage[type, priority] = value; }
    }

    public WorkDictionary()
    {
        int enumCount = (int)WorkType.Construction + 1;

        for (int i = 0; i < enumCount; i++) 
        {
            for (int j = 0; j < PRIORITY_COUNT; j++) 
            {
                _storage.Add((WorkType)i, j, new List<Work>(8));
            }
        }
    }

    public void Add(Work work)
    {
        WorkType type = work.WorkType;
        int priority = work.Priority;

        _storage[type][priority].Add(work);
    }

    public void Remove(Work work)
    {
        WorkType type = work.WorkType;
        int priority = work.Priority;

        _storage[type][priority].Remove(work);
    }

    public void ChangePriority(Work work, int priority)
    {
        WorkType type = work.WorkType;
        int beforePriority = work.Priority;

        _storage[type][beforePriority].Remove(work);
        _storage[type][priority].Add(work);
    }
}
