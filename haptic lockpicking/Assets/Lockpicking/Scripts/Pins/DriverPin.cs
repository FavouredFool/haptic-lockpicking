using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverPin : Pin
{
    public bool IsBelowSheer()
    {
        return GetUpperEdgePosition() <= PinController.SHEERLINE_HEIGHT;
    }

    public bool IsOnSheer(PinController pinController)
    {
        bool aboveThreshold = GetUpperEdgePosition() >= PinController.SHEERLINE_HEIGHT - pinController.GetSetThreshold();

        return IsBelowSheer() && aboveThreshold;
    }

    public float GetUpperEdgePosition()
    {
        return transform.position.y + PinController.CONSTANT_DRIVER_OFFSET;
    }



}
