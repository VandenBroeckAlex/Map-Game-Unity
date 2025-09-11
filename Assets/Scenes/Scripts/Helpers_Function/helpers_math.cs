using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class helpers_math : MonoBehaviour
{
    public static float RoundToTwoDecimals(float input)
    {
        return (float)MathF.Round(input, 2);
    }
}
