using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyPin : Pin
{
    [SerializeField]
    LayerMask _pickLayer;


    bool _isBeingTouched = false;



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
    }

    void OnCollisionExit(Collision other)
    {
        if (_pickLayer == (_pickLayer | (1 << other.gameObject.layer)))
        {
            _isBeingTouched = false;
        }
    }
}
