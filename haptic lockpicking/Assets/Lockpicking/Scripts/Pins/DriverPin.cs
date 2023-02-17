using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverPin : Pin
{
    float _belowSheerHeight;
    public void Start()
    {
        _belowSheerHeight = PinController.SHEERLINE_HEIGHT - PinController.CONSTANT_DRIVER_OFFSET;
    }

    public override void AnyStateUpdate(PinController pinController)
    {
    }

    public override void LooseUpdate(PinController pinController)
    {

    }

    public override void MovableUpdate(PinController pinController)
    {

    }

    public override void LockedUpdate(PinController pinController)
    {

    }

    public bool IsBelowSheer()
    {
        return transform.position.y + PinController.CONSTANT_DRIVER_OFFSET <= PinController.SHEERLINE_HEIGHT;
    }



}
