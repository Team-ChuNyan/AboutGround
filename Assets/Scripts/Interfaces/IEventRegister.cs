using System;

public interface IEventRegister
{
    public void RegisterEvent(Action action);
    public void UnregisterEvent(Action action);
    public void ClearEvent();
}
