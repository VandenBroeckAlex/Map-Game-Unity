
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

// Throw immediately
public class ThrowErrorHandler : IResolutionErrorHandler
{
    public int HandleMissingId<T>(string tag, string context)
    {
        throw new KeyNotFoundException($"[CRITICAL] {typeof(T).Name} Tag '{tag}' not found in {context}.");
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

    public int HandleMissingId<T>(string tag, string context)
    {
        return _fallbackId;
        //throw new Exception($"Fallback ID '{_fallbackId}' also missing! Check your core data.");
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
    public int HandleMissingId<T>(string tag, string context)
    {
        string logLine = $"Missing {typeof(T).Name}: '{tag}' (Source: {context})";
        File.AppendAllLines(_logPath, new[] { logLine });

        // Return the first available item or null as a secondary fallback
        return 0;
    }

    public void RaiseError(string context)
    {
        string logLine = context;
        File.AppendAllLines(_logPath, new[] { logLine });
    }
}