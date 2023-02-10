using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController : MonoBehaviour
{
    [SerializeField]
    private Transform _driver;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private MeshCollider _meshCollider;

    [SerializeField]
    private Camera _camera;

    [SerializeField, Range(1, 20)]
    private float _speedMultiplicator;

    [SerializeField, Range(0, 5)]
    private float _upBoundsThreshold;

    [SerializeField, Range(0, 5)]
    private float _downBoundsThreshold;

    [SerializeField, Range(0, 5)]
    private float _forwardBoundsThreshold;

    [SerializeField, Range(0, 5)]
    private float _backBoundsThreshold;

    [SerializeField, Range(0, 20)]
    private float _rotationAngleThreshold;

    Vector3 _startPosition;

    Vector3 _positionOffset = Vector3.zero;
    Quaternion _rotationOffset = Quaternion.identity;

    Quaternion _startRotation;

    Quaternion _viewRotation;

    public void Awake()
    {
        _meshRenderer.enabled = false;
        _meshCollider.enabled = false;

        _startPosition = transform.position;

        _startRotation = transform.rotation;

        _viewRotation = _camera.transform.rotation * Quaternion.Euler(new Vector3(180, 0, 180));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Recalibrate();
        }

        transform.SetLocalPositionAndRotation(CalculatePosition(), CalculateRotation());
    }

    public void Recalibrate()
    {
        _meshRenderer.enabled = true;
        _meshCollider.enabled = true;
        
        _positionOffset = ((_viewRotation * _driver.position) * _speedMultiplicator) - _startPosition;

        _rotationOffset = Quaternion.Inverse(_driver.transform.rotation) * _startRotation;
    }

    public Quaternion CalculateRotation()
    {
        Quaternion absoluteRotation = _driver.rotation * _rotationOffset;
        Swing_Twist_Decomposition(absoluteRotation, Vector3.right, out Quaternion swing, out Quaternion twist);
        return Quaternion.RotateTowards(_startRotation, twist, _rotationAngleThreshold);
    }

    public Vector3 CalculatePosition()
    {
        Vector3 absolutePosition = ((_viewRotation * _driver.position) * _speedMultiplicator) - _positionOffset;


        float positionUp = Vector3.Dot(Vector3.up, absolutePosition);
        float _clampedUp = Mathf.Clamp(positionUp, _startPosition.y - _downBoundsThreshold, _startPosition.y + _upBoundsThreshold);

        float positionForward = Vector3.Dot(Vector3.forward, absolutePosition);
        float _clampedForward = Mathf.Clamp(positionForward, _startPosition.z - _backBoundsThreshold, _startPosition.z + _forwardBoundsThreshold);

        return new(0, _clampedUp, _clampedForward);
    }


    /**
     * https://stackoverflow.com/questions/3684269/component-of-a-quaternion-rotation-around-an-axis
   Decompose the rotation on to 2 parts.
   1. Twist - rotation around the "direction" vector
   2. Swing - rotation around axis that is perpendicular to "direction" vector
   The rotation can be composed back by 
   rotation = swing * twist

   has singularity in case of swing_rotation close to 180 degrees rotation.
   if the input quaternion is of non-unit length, the outputs are non-unit as well
   otherwise, outputs are both unit
    */

    void Swing_Twist_Decomposition(Quaternion rotation, Vector3 direction, out Quaternion swing, out Quaternion twist)
    {
        Vector3 rotationVector = new(rotation.x, rotation.y, rotation.z);
        Vector3 p = Vector3.Project(rotationVector, direction);
        // if dot product is negative -> Negate all 4 components of twist
        // this is doubled because the project should give the dot directly
        float dot = Vector3.Dot(rotationVector, direction);

        twist = new Quaternion();
        if (dot >= 0)
        {
            twist.Set(p.x, p.y, p.z, rotation.w);
        }
        else
        {
            twist.Set(-p.x, -p.y, -p.z, -rotation.w);
        }

        
        twist.Normalize();

        // Uses Unity Mathematics Library
        swing = rotation * Unity.Mathematics.math.conjugate(twist);

    }


}
