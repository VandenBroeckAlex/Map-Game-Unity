using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct FCurentMax 
{
    public float Current;
    public float Max;

    public void SetCurrent(float value) => Current = Math.Clamp(value, 0, Max);

    public FCurentMax(float current, float max)
    {
        this.Current = current;
        Max = max;
    }
}
