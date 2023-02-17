using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyPin : Pin
{
    private bool _belowSheer = false;


    public override void AnyStateUpdate(PinController pinController)
    {
        _belowSheer = (transform.position.y < PinController.SHEERLINE_HEIGHT);
    }

    public override void LooseUpdate(PinController pinController)
    {
        //_rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.Force);
        _rigidbody.AddForce(new Vector3(0, -0.1f, 0), ForceMode.Force);
    }

    public override void MovableUpdate(PinController pinController)
    {
        //_rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.Force);
    }

    public override void LockedUpdate(PinController pinController)
    {

    }

    public bool GetBelowSheer()
    {
        return _belowSheer;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
}
