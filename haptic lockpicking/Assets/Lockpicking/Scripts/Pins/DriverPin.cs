using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverPin : Pin
{
    float _belowSheerHeight;

    public override void Start()
    {
        base.Start();
        _belowSheerHeight = PinController.SHEERLINE_HEIGHT - PinController.CONSTANT_DRIVER_OFFSET;
    }

    public override void AnyStateUpdate(PinController pinController)
    {
    }

    public override void LooseUpdate(PinController pinController)
    {
        _rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.Force);
        _rigidbody.AddForce(new Vector3(0, -0.1f, 0), ForceMode.Force);

        if (transform.position.y < _belowSheerHeight)
        {
            pinController.ActivateDriverPinBlockade();
        }
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
