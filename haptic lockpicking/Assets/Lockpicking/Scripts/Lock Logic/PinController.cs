using UnityEngine;

public class PinController : MonoBehaviour
{
    public enum TensionState { LOOSE, MOVABLE, LOCKED };

    public enum PinState { MOVING, STATIC }

    [SerializeField]
    private KeyPin _keyPin;

    [SerializeField]
    private DriverPin _driverPin;

    [SerializeField]
    private BoxCollider _driverPinBlockade;

    [SerializeField]
    private Transform _screw;

    [SerializeField, Range(0, 1)]
    private float _maxVelocityForSet = 0.25f;

    public static float CONSTANT_DRIVER_OFFSET = 0.53f;

    public static float CONSTANT_SCREW_OFFSET = 1.448f;

    public static float CONSTANT_SCREW_MULTIPLIER = 0.687f;

    public static float SHEERLINE_HEIGHT = -0.99f;

    private bool _isOpen = false;

    private TensionState _tensionState = TensionState.LOOSE;

    public void Awake()
    {
        _driverPinBlockade.enabled = false;
    }

    public void FixedUpdate()
    {
        
        SwitchPinState();
        UpdateDriverPinBlockade();

    }

    public void Update()
    {

        // Check if Lock is openable
        // Should check if overset
        _isOpen = _driverPin.IsBelowSheer();


        // Screw is always attached
        float offset = _driverPin.transform.position.y - _screw.transform.position.y;
        float diff = offset - CONSTANT_SCREW_OFFSET;
        _screw.transform.localScale = new Vector3(1f, 1 + diff * CONSTANT_SCREW_MULTIPLIER, 1f);
    }

    protected void SwitchPinState()
    {
        switch (_tensionState)
        {
            case TensionState.LOOSE:
            case TensionState.MOVABLE:
                _keyPin.PhysicsUpdate();
                _driverPin.PhysicsUpdate();
                break;
            case TensionState.LOCKED:
                _keyPin.PhysicsStop();
                _driverPin.PhysicsStop();
                break;
        }


    }

    public void UpdateDriverPinBlockade()
    {
        if (_tensionState == TensionState.LOOSE)
        {
            DriverPinBlockadeActive(false);
        }
        else if (_driverPin.IsBelowSheer() && Mathf.Abs(_driverPin.GetVelocity()) <= _maxVelocityForSet)
        {
            DriverPinBlockadeActive(true);
        } 
        else
        {
            DriverPinBlockadeActive(false);
        }
    }

    public void SetTensionState(TensionState tensionState)
    {
        _tensionState = tensionState;
    }

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public KeyPin GetKeyPin()
    {
        return _keyPin;
    }

    public float GetKeyPinHeight()
    {
        return _keyPin.transform.position.y;
    }

    public void DriverPinBlockadeActive(bool active)
    {
        _driverPinBlockade.enabled = active;
    }
}
