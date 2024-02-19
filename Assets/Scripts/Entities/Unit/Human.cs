public class Human : Unit, IWorkable, IAttackable
{
    public override void Awake()
    {
        base.Awake();
        UnitData.SetRace(RaceType.Human);
    }

    public void Work(Work work)
    {
        // TODO : 일하는 기능
    }

    public bool IsPossibleToWork()
    {
        // TODO :  작업 가능 여부를 체크
        return true;
    }

    public bool IsPossibleToWork(WorkType type)
    {
        // TODO :  해당 유형의 작업이 가능한지
        return true;
    }
}
