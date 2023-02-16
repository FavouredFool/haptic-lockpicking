using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPin : MonoBehaviour
{
    private Rigidbody _rigidbody;


    private bool _belowSheer = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        _rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.Force);

        _belowSheer = (transform.position.y < PinController.SHEERLINE_HEIGHT);
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
