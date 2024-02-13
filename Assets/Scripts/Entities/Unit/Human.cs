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

    public bool IsPossible()
    {
        // TODO :  작업 가능 여부를 체크
        return true;
    }
}