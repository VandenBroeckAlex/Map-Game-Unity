

public interface IResolutionErrorHandler
{
    int HandleMissingId(string context);
    void RaiseError(string context);

    void Beggin(string context);
    void End();
}

