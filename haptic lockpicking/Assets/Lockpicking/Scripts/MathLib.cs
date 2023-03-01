using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathLib
{
    public static float Remap(float value, float fromlow, float fromhigh, float tolow, float tohigh)
    {
        // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        float res = (value - fromlow) / (fromhigh - fromlow) * (tohigh - tolow) + tolow;

        if (res.Equals(float.NaN))
        {
            res = tolow;
        }

        return res;

    }
}
