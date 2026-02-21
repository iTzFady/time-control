using System.Collections.Generic;
using UnityEngine;

public class TimeBuffer<T>
{
    private List<T> buffer = new();
    private int maxFrames;

    public TimeBuffer(int maxFrames)
    {
        this.maxFrames = maxFrames;
    }
    public void Record(T state)
    {
        if (buffer.Count >= maxFrames)
            buffer.RemoveAt(0);
        buffer.Add(state);
    }
    public T PopLast()
    {
        if (buffer.Count == 0)
            return default;
        T state = buffer[^1];
        buffer.RemoveAt(buffer.Count - 1);
        return state;
    }
    public int Count => buffer.Count;
}
