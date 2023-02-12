using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandMovement : MonoBehaviour
{
    [SerializeField]
    private Transform _driver;

    [SerializeField]
    private GameObject _hand;

    [SerializeField]
    private Camera _camera;

    [SerializeField, Range(1, 20)]
    private float _speedMultiplicator;

    Vector3 _startPosition;

    Vector3 _positionOffset = Vector3.zero;
    Quaternion _rotationOffset = Quaternion.identity;

    Quaternion _startRotation;

    Quaternion _viewRotation;

    public void Awake()
    {
        _hand.SetActive(false);

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
        _hand.SetActive(true);

        _positionOffset = ((_viewRotation * _driver.position) * _speedMultiplicator) - _startPosition;

        _rotationOffset = Quaternion.Inverse(_driver.transform.rotation) * _startRotation;
    }

    public Quaternion CalculateRotation()
    {
        Quaternion absoluteRotation = _viewRotation * _driver.rotation * _rotationOffset;
        return absoluteRotation;
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

    public Vector3 CalculatePosition()
    {
        return ((_viewRotation * _driver.position) * _speedMultiplicator) - _positionOffset;
    }
}
