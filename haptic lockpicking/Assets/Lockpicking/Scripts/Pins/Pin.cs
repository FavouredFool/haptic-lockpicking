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

    private RigidbodyConstraints _freezeAllButYPos = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

    MeshRenderer _meshRenderer;

    Color _defaultColor;

    public virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultColor = _meshRenderer.material.color;
    }

    public void PhysicsUpdate()
    {
        float force = _springForce + -_gravityForce;

        Vector3 forceGlobalSpace = transform.TransformDirection(new Vector3(0, force, 0));
        _rigidbody.AddForce(forceGlobalSpace, ForceMode.Force);
    }

    public PinController GetPinController()
    {
        return transform.parent.GetComponent<PinController>();
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.y;
    }

    public void FreezePinMovement()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ActivatePinMovement()
    {
        _rigidbody.constraints = _freezeAllButYPos;
    }

    public void ChangeColor(Color color)
    {
        if(!_meshRenderer)
        {
            return;
        }

        _meshRenderer.material.color = color;
    }

    public Color GetDefaultColor()
    {
        return _defaultColor;
    }



}
