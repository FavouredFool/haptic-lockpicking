using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathLib
{
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        float res = (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        if (res.Equals(float.NaN))
        {
            res = 0;
        }

        return res;

    }
}
