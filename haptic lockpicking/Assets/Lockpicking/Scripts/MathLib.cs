using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathLib
{
    public static float Remap(float value, float a1, float a2, float b1, float b2)
    {
        float res = (value - a1) / (a2 - a1) * (b2 - b1) + b1;

        if (res.Equals(float.NaN))
        {
            res = b1;
        }

        return res;

    }
}
