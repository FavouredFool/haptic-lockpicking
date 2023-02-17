using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pin : MonoBehaviour
{
    protected Rigidbody _rigidbody;

    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        // Freeze local x and z, but not y
        Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
        localVelocity.x = 0;
        localVelocity.z = 0;
        _rigidbody.velocity = transform.TransformDirection(localVelocity);
    }

    public abstract void AnyStateUpdate(PinController pinController);

    public abstract void LooseUpdate(PinController pinController);

    public abstract void MovableUpdate(PinController pinController);

    public abstract void LockedUpdate(PinController pinController);


}
