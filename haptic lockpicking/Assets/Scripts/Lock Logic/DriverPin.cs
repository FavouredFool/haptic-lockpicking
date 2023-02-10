using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverPin : MonoBehaviour
{

    private float _maxDriverHeight = float.PositiveInfinity;


    public void SetAttachedPosition(Vector3 keyPinPosition)
    {
        float yPosition = Mathf.Min(keyPinPosition.y - PinController.CONSTANT_DRIVER_OFFSET, _maxDriverHeight);

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    public void SetMaxDriverHeight(float maxDriverHeight)
    {
        _maxDriverHeight = maxDriverHeight;
    }

    public float GetMaxDriverHeight()
    {
        return _maxDriverHeight;
    }



}
