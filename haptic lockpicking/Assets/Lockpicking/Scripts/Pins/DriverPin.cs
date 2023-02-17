using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverPin : Pin
{
    public bool IsBelowSheer()
    {
        return transform.position.y + PinController.CONSTANT_DRIVER_OFFSET <= PinController.SHEERLINE_HEIGHT;
    }



}
