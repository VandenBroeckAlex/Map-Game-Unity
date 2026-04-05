using UnityEngine;

public interface IDataLoader
{
    void Load(DataRegistery registry, IResolutionErrorHandler errorHandler);
}
