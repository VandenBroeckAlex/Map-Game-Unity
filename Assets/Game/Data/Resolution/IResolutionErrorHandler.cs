using System.Collections.Generic;
using UnityEngine;

public interface IResolutionErrorHandler
{
    int HandleMissingId<T>(string id, string context);
    void RaiseError(string context);
}

