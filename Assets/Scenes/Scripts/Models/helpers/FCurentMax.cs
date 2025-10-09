using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct FCurentMax 
{
    public float current;
    public float max;

    public void SetCurrent(float value) => current = Math.Clamp(value, 0, max);

    public FCurentMax(float current, float max)
    {
        this.current = current;
        this.max = max;
    }
}
