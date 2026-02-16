

using System;
using System.Collections;
using System.Collections.Generic;

public class IntentBuffer : IIntentBuffer
{
    private readonly Dictionary<Type, IList> _buffers = new();
    public void Clear<T>()
    {
        if (_buffers.TryGetValue(typeof(T), out var list))
            list.Clear();
    }

    public IReadOnlyList<T> Collect<T>()
    {
        if (_buffers.TryGetValue(typeof(T), out var list))
            return (IReadOnlyList<T>)list;

        return Array.Empty<T>();
    }

    public void Enqueue<T>(T intent)
    {
        if (!_buffers.TryGetValue(typeof(T), out var list))
        {
            list = new List<T>();
            _buffers[typeof(T)] = list;
        }
        list.Add(intent);
    }
}