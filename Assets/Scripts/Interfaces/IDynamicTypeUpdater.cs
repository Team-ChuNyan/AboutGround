using System;

public interface IDynamicTypeUpdater<T> : IEventRegister where T : Enum
{
    public void UpdateFormType(T type);
}
