using UnityEngine;

public interface IDataLoader
{
    void Load(LoaderDataRegistery registry, IResolutionErrorHandler errorHandler);
}
