using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickHead : MonoBehaviour
{
    [SerializeField]
    LayerMask _keyPinLayer;

    [SerializeField]
    LayerMask _keyPinBorder;

    [SerializeField]
    PickController _pickController;

    private void OnTriggerEnter(Collider other)
    {
        if (_keyPinBorder == (_keyPinBorder | (1 << other.gameObject.layer)))
        {
            Debug.Log("ENTER Border");
            _pickController.SetIsInsidePin(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_keyPinBorder == (_keyPinBorder | (1 << other.gameObject.layer)))
        {
            Debug.Log("EXIT Border");
            _pickController.SetIsInsidePin(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other);
        if (_keyPinLayer == (_keyPinLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("ENTER Pin");
            _pickController.SetTouchedPin(other.gameObject.GetComponent<KeyPin>());
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (_keyPinLayer == (_keyPinLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("EXIT Pin");
            _pickController.SetTouchedPin(null);
        }
    }
}
