using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyPin : Pin
{
    [SerializeField]
    LayerMask _pickLayer;

    [SerializeField]
    LayerMask _KeyPinBlockadeLayer;


    bool _isBeingTouched = false;

    float _maxVelocity = 0;


    private void FixedUpdate()
    {
        float velocity = _rigidbody.velocity.y;

        if (_maxVelocity < velocity)
        {
            _maxVelocity = velocity;
        }
    }

    public void OversetUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    public bool IsBelowSheer()
    {
        return transform.position.y < PinController.SHEERLINE_HEIGHT;
    }

    public bool IsBeingTouched()
    {
        return _isBeingTouched;
    }

    void OnCollisionEnter(Collision other)
    {
        
        if (_pickLayer == (_pickLayer | (1<<other.gameObject.layer)))
        {
            _isBeingTouched = true;
        }

        if (_KeyPinBlockadeLayer == (_KeyPinBlockadeLayer | (1 << other.gameObject.layer)))
        {
            if (_maxVelocity >= PinManager.Instance.GetVelocityThresholdForPushupSound())
            {
                AudioManager.Instance.PlayWithVolume("Pin_Goes_Up", MathLib.Remap(_maxVelocity, 0, 15, 0, 1));
            }
            
            _maxVelocity = 0;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (_pickLayer == (_pickLayer | (1 << other.gameObject.layer)))
        {
            _isBeingTouched = false;
        }
    }
}
