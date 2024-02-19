public interface IWorkable
{
    public void Work(Work work);
    public bool IsPossibleToWork();
    public bool IsPossibleToWork(WorkType type);
}
