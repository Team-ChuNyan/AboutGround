public interface IObjectStorage<T>
{
    void AddObject(T item);
    void RemoveObject(T item);
}
