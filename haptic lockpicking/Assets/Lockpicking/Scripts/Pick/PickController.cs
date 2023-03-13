using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickController : MonoBehaviour
{
    readonly float CIRCLERADIUSFOROFFSET = 1.7f;

    [Header("KeyCollisionStuff")]

    [SerializeField]
    float _velocityThreshold = 0.125f;

    [SerializeField]
    int _frameAmountUntilThumb = 20;

    [Header("Other Stuff")]
    [SerializeField]
    Transform _trackerAttachPoint;

    [SerializeField]
    LockController _lock;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private MeshCollider _meshCollider;

    [SerializeField]
    private GameObject _pickIndicatorCanvas;

    [SerializeField]
    private LayerMask _borderLayer;

    [SerializeField]
    private float _physicsMoveSpeed = 1;

    [SerializeField]
    private float _physicsRotateSpeed = 1;

    [SerializeField, Range(0, 1)]
    private float _deltaMoveThreshold = 0.25f;

    [SerializeField, Range(0, 1)]
    private float _deltaRotateThreshold = 0.25f;

    [SerializeField, Range(1, 100)]
    private float _distanceMultiplicator;

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

    KeyPin _touchedKeyPin;

    Vector3 _startPosition;

    Vector3 _positionOffset = Vector3.zero;
    Quaternion _rotationOffset = Quaternion.identity;

    Quaternion _startRotation;

    Quaternion _viewRotation;

    Rigidbody _rigidBody;

    int _collideAmount = 0;

    Vector3 _pickPosition = Vector3.zero;
    Quaternion _pickRotation = Quaternion.identity;

    bool _isInsidePin = false;

    Vector3 _rotatedTrackerPosition = Vector3.zero;

    Vector3 _absolutePosition = Vector3.zero;

    public void Awake()
    {
        _startPosition = transform.position;

        _startRotation = transform.rotation;

        _viewRotation = CameraManager.Instance.GetCamera().transform.rotation * Quaternion.Euler(new Vector3(180, 0, 180));

        _rigidBody = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        _pickRotation = CalculateRotation();
        _pickPosition = CalculatePosition(_pickRotation);
        
        transform.position = _pickPosition;
        transform.rotation = _pickRotation;
    }

    void Vibrate()
    {
        if (LockManager.Lock == null)
        {
            return;
        }


        if (_isInsidePin)
        {
            PickVibrationManager.Instance.SetInsidePinVibration();
            return;
        }

        if (_touchedKeyPin == null)
        {
            return;
        }


        int vibrationIntensity;

        switch(_touchedKeyPin.GetPinController().GetPinState())
        {
            case PinController.PinState.SPRINGY:
                vibrationIntensity = PickVibrationManager.Instance.GetSpringyIntensity();
                break;
            case PinController.PinState.BINDING:

                if (PinManager.Instance.GetRespectOrder())
                {
                    vibrationIntensity = PickVibrationManager.Instance.GetBindingIntensity();
                }
                else
                {
                    vibrationIntensity = PickVibrationManager.Instance.GetSpringyIntensity();
                }

                break;
            default:
                vibrationIntensity = 0;
                break;
        }

        PickVibrationManager.Instance.SetVibrationThisFrame(vibrationIntensity);
    }

    public void FixedUpdate()
    {
        MovePick();
        Vibrate();
    }

    public void MovePick()
    {
        Quaternion goalRotation = CalculateRotation();
        Vector3 goalPosition = CalculatePosition(goalRotation);
        
        /*
        float keepZ = _pickPosition.z;

        if (Vector3.Distance(_pickPosition, goalPosition) > _deltaMoveThreshold)
        {
            _pickPosition = Vector3.MoveTowards(_pickPosition, goalPosition, _physicsMoveSpeed);
        }

        if (1 - Quaternion.Dot(_pickRotation, goalRotation) > _deltaRotateThreshold)
        {
            _pickRotation = Quaternion.RotateTowards(_pickRotation, goalRotation, _physicsRotateSpeed);
        }

        if (_collideAmount > 0)
        {
            // TODO: Das bringt relativ wenig wenn der Pin sich sehr rapide bewegt. Dann hat er schon zwei Pins durchstochen und bleibt erst dann stehen.
            _pickPosition.z = Mathf.Max(_pickPosition.z, keepZ);
        }
        */


        _rigidBody.MovePosition(goalPosition);
        _rigidBody.MoveRotation(goalRotation);
    }



    public void Recalibrate()
    {
        _positionOffset = ((_viewRotation * PickManager.Instance.GetPickDriver().position) * _distanceMultiplicator) - _startPosition;

        _rotationOffset = Quaternion.Inverse(PickManager.Instance.GetPickDriver().transform.rotation) * _startRotation;
    }

    public Quaternion CalculateRotation()
    {
        Quaternion absoluteRotation = PickManager.Instance.GetPickDriver().rotation * _rotationOffset;
        Swing_Twist_Decomposition(absoluteRotation, Vector3.right, out Quaternion swing, out Quaternion twist);
        return Quaternion.RotateTowards(_startRotation, twist, float.PositiveInfinity);
    }

    public Vector3 CalculatePosition(Quaternion goalRotation)
    {
        Vector3 threeD_Position = ((_viewRotation * PickManager.Instance.GetPickDriver().position) * _distanceMultiplicator) - _positionOffset;

        float positionUp = Vector3.Dot(Vector3.up, threeD_Position);
        float positionForward = Vector3.Dot(Vector3.forward, threeD_Position);

        _absolutePosition = new Vector3(0, positionUp, positionForward);


        // hier muss die Rotation eingerechnet werden, da durch den Offset zwischen Pivot und Tracker Rotation des Pivots zu Bewegungsänderung des Trackers führt

        Vector3 baseTrackerPosition = _absolutePosition + new Vector3(0, 0, CIRCLERADIUSFOROFFSET);
        _rotatedTrackerPosition = RotatePointAroundPivot(baseTrackerPosition, _absolutePosition, goalRotation);

        Vector3 direction = _rotatedTrackerPosition - _absolutePosition;

        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.left);

        float a = Mathf.Sin(Mathf.Deg2Rad * angle) * CIRCLERADIUSFOROFFSET;
        float b = Mathf.Cos(Mathf.Deg2Rad * angle) * CIRCLERADIUSFOROFFSET;

        float verticalOffset = a;
        float horizontalOffset = CIRCLERADIUSFOROFFSET - b;

        Vector3 offsettedPosition = new Vector3(0, _absolutePosition.y - verticalOffset, _absolutePosition.z + horizontalOffset);

        return offsettedPosition;
        
        float _clampedUp = Mathf.Clamp(positionUp, _startPosition.y - _downBoundsThreshold, _startPosition.y + _upBoundsThreshold);
        float _clampedForward = Mathf.Clamp(positionForward, _startPosition.z - _backBoundsThreshold, _startPosition.z + _forwardBoundsThreshold);

        //return new(0, _clampedUp, _clampedForward);
        
    }

    Vector3 RotatePointAroundPivot2D(Vector2 point, Vector2 pivot, Quaternion rotation)
    {
        // https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
        Vector2 dir = point - pivot; // get point direction relative to pivot
        dir = rotation * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        // https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = rotation * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
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

    private void OnTriggerEnter(Collider collider)
    {
        if (_borderLayer == (_borderLayer | (1 << collider.gameObject.layer)))
        {
            AudioManager.Instance.Play("Pick_Hits_Pin");
            _collideAmount += 1;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (_borderLayer == (_borderLayer | (1 << collider.gameObject.layer)))
        {
            _collideAmount -= 1;
        }
    }

    public GameObject GetPickIndicatorCanvas()
    {
        return _pickIndicatorCanvas;
    }

    public void SetTouchedPin(KeyPin pin)
    {
        _touchedKeyPin = pin;
    }

    public void SetIsInsidePin(bool isInsidePin)
    {
        _isInsidePin = isInsidePin;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_absolutePosition, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
        //Gizmos.DrawWireSphere(_rotatedTrackerPosition, 1);
    }
}
