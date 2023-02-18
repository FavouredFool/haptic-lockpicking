using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pin : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    float _gravityForce = 9.81f;

    [SerializeField, Range(0, 100)]
    float _springForce = 20f;

    protected Rigidbody _rigidbody;

    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void PhysicsUpdate()
    {
        _rigidbody.isKinematic = false;
        float force = _springForce + -_gravityForce;

        Vector3 forceGlobalSpace = transform.TransformDirection(new Vector3(0, force, 0));
        _rigidbody.AddForce(forceGlobalSpace, ForceMode.Force);
    }

    public void PhysicsStop()
    {
        _rigidbody.isKinematic = true;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.y;
    }

}
