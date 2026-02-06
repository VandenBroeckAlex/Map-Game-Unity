using System;

[System.Serializable]
public class FloatCurentMax
{
    public float current;
    public float max;

    public void SetCurrent(float value) => current = Math.Clamp(value, 0, max);

    public FloatCurentMax(float current, float max)
    {
        this.current = current;
        this.max = max;
    }
}

