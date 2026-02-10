using System.Collections.Generic;


public interface IIntentBuffer
{
    void Enqueue<T>(T intent);
    IReadOnlyList<T> Collect<T>();
    void Clear<T>();
}
