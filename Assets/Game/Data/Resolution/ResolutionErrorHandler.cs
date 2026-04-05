

using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine.UIElements;


// Throw immediately
public class ThrowErrorHandler : IResolutionErrorHandler
{
    public int HandleMissingId(string context)
    {
        throw new KeyNotFoundException(context);
    }

    public void Beggin(string context)
    {
        //Used for text log organisation 
    }

    public void End()
    {
        //Used for text log organisation 
    }

    public void RaiseError(string context)
    {
        throw new InvalidDataException(context);
    }
}

// Use a fallback and keep going
public class DefaultValueErrorHandler : IResolutionErrorHandler
{
    private readonly int _fallbackId;
    public DefaultValueErrorHandler(int fallbackId = 0) => _fallbackId = fallbackId; 

    public int HandleMissingId(string context)
    {
        return _fallbackId;
        //throw new Exception($"Fallback ID '{_fallbackId}' also missing! Check your core data.");
    }

    public void Beggin(string context)
    {
        //Used for text log organisation 
    }

    public void End()
    {
        //Used for text log organisation 
    }

    public void RaiseError(string context)
    {
        throw new InvalidDataException(context);
    }
}

//  Log to a file and return fallback
public class LoggingErrorHandler : IResolutionErrorHandler
{
    private readonly string _logPath = "data_errors.txt";
    public int HandleMissingId(string context)
    {
        string logLine = context;
        File.AppendAllLines(_logPath, new[] { logLine });

        // Return the first available item or null as a secondary fallback
        return 0;
    }

    public void Beggin(string context)
    {
        string logLine = $"{context} :";
        File.AppendAllLines(_logPath, new[] { logLine });
    }

    public void End()
    {
        string logLine = $"--- --- --- ---";
        File.AppendAllLines(_logPath, new[] { logLine });
    }

    public void RaiseError(string context)
    {
        string logLine = context;
        File.AppendAllLines(_logPath, new[] { logLine });
    }
}
//Loader X :
//---