using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickController : MonoBehaviour
{
    [Header("KeyCollisionStuff")]
    [SerializeField]
    LayerMask _keyPinLayer;

    [SerializeField]
    float _velocityThreshold = 0.125f;

    [SerializeField]
    int _frameAmountUntilThumb = 20;

    [Header("Other Stuff")]
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

    List<KeyPin> _touchedKeyPins = new();

    Vector3 _startPosition;

    Vector3 _positionOffset = Vector3.zero;
    Quaternion _rotationOffset = Quaternion.identity;

    Quaternion _startRotation;

    Quaternion _viewRotation;

    Rigidbody _rigidBody;

    int _collideAmount = 0;

    Vector3 _pickPosition = Vector3.zero;
    Quaternion _pickRotation = Quaternion.identity;

    int _frameCountForThumperBuzz = 0;

    public void Awake()
    {
        _startPosition = transform.position;

        _startRotation = transform.rotation;

        _viewRotation = CameraManager.Instance.GetCamera().transform.rotation * Quaternion.Euler(new Vector3(180, 0, 180));

        _rigidBody = GetComponent<Rigidbody>();

        
    }

    public void OnEnable()
    {
        _pickPosition = CalculatePosition();
        _pickRotation = CalculateRotation();

        transform.position = _pickPosition;
        transform.rotation = _pickRotation;
    }

    void Vibrate()
    {
        if (LockManager.Lock == null)
        {
            return;
        }


        // 1. Get all active collisions with keypins
        // 2. Only take pins into account who's Velocity is higher than the threshold.
        // 3. For each, add a set amount of intensity. Loose: 10, Binding: 40

        int vibrationIntensity = 0;
        bool touchingSet = _touchedKeyPins.Any(e => e.GetPinController().GetPinState() == PinController.PinState.SET);
        if (touchingSet)
        {
            _frameCountForThumperBuzz += 1;
        }
        else
        {
            _frameCountForThumperBuzz = 0;
        }

        foreach (KeyPin pin in _touchedKeyPins)
        {
            int additionalIntensity = 0;
            switch (pin.GetPinController().GetPinState())
            {
                case PinController.PinState.SPRINGY:
                    additionalIntensity = 10;
                    break;
                case PinController.PinState.BINDING:
                    additionalIntensity = 40;
                    break;
                case PinController.PinState.SET:
                    additionalIntensity = 110;
                    break;
            }

            vibrationIntensity += additionalIntensity;
            
        }

        if (touchingSet)
        {
            if (_frameCountForThumperBuzz >= _frameAmountUntilThumb)
            {
                PickVibrationManager.Instance.SetInsidePinVibration();
            }
        }
        else
        {
            PickVibrationManager.Instance.SetVibrationThisFrame(vibrationIntensity);
        }
        

        
    }

    public void FixedUpdate()
    {
        Vector3 goalPosition = CalculatePosition();
        Quaternion goalRotation = CalculateRotation();

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

        _rigidBody.MovePosition(_pickPosition);
        _rigidBody.MoveRotation(_pickRotation);


        Vibrate();

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
        return Quaternion.RotateTowards(_startRotation, twist, _rotationAngleThreshold);
    }

    public Vector3 CalculatePosition()
    {
        Vector3 absolutePosition = ((_viewRotation * PickManager.Instance.GetPickDriver().position) * _distanceMultiplicator) - _positionOffset;


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

    void OnCollisionEnter(Collision other)
    {
        if (_keyPinLayer == (_keyPinLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("ENTER");
            _touchedKeyPins.Add(other.gameObject.GetComponent<KeyPin>());
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (_keyPinLayer == (_keyPinLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("EXIT");
            _touchedKeyPins.Remove(other.gameObject.GetComponent<KeyPin>());
        }
    }


}
