using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverPin : Pin
{
    [SerializeField]
    LayerMask keyPinBottomMask;


    BoxCollider _collider;

    bool _isTouching = false;

    public override void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider>();
    }

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

    public void OversetUpdate()
    {
        if (_isTouching)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        PhysicsUpdate();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (keyPinBottomMask == (keyPinBottomMask | (1 << collision.gameObject.layer)))
        {
            _isTouching = true;
        }

    }

    public void OnCollisionExit(Collision collision)
    {
        if (keyPinBottomMask == (keyPinBottomMask | (1 << collision.gameObject.layer)))
        {
            _isTouching = false;
        }
    }

}
