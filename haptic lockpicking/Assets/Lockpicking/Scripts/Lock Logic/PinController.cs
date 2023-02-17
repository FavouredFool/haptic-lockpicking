using UnityEngine;

public class PinController : MonoBehaviour
{
    public enum PinState { LOOSE, MOVABLE, LOCKED };

    [SerializeField]
    private KeyPin _keyPin;

    [SerializeField]
    private DriverPin _driverPin;

    [SerializeField]
    private BoxCollider _driverPinBlockade;

    [SerializeField]
    private Transform _screw;

    public static float CONSTANT_DRIVER_OFFSET = 0.53f;

    public static float CONSTANT_SCREW_OFFSET = 1.448f;

    public static float CONSTANT_SCREW_MULTIPLIER = 0.687f;

    public static float SHEERLINE_HEIGHT = -0.99f;

    private bool _isOpen = false;

    private bool _keyPinStop;

    private bool _driverPinStop;

    private PinState _pinState = PinState.LOOSE;

    public void Awake()
    {
        _driverPinBlockade.enabled = false;
    }

    public void FixedUpdate()
    {
        if (_keyPinStop)
        {
            return;
        }

        SwitchPinState();
    }

    public void Update()
    {
        // This is still a bit wonky when it comes to coding
        if (_driverPin.IsBelowSheer() && _pinState == PinState.MOVABLE)
        {
            DriverPinBlockadeActive(true);
        }
        else if (_pinState == PinState.LOOSE)
        {
            DriverPinBlockadeActive(false);
        }

        // Check if Lock is openable
        _isOpen = _driverPin.IsBelowSheer() && _pinState == PinState.MOVABLE;


        // Screw is always attached
        float offset = _driverPin.transform.position.y - _screw.transform.position.y;
        float diff = offset - CONSTANT_SCREW_OFFSET;
        _screw.transform.localScale = new Vector3(1f, 1 + diff * CONSTANT_SCREW_MULTIPLIER, 1f);
    }

    protected void SwitchPinState()
    {
        switch (_pinState)
        {
            case PinState.LOOSE:
            case PinState.MOVABLE:
                _keyPin.PhysicsUpdate();
                _driverPin.PhysicsUpdate();
                break;
            case PinState.LOCKED:
                _keyPin.PhysicsStop();
                _driverPin.PhysicsStop();
                break;
        }
    }

    public void SetPinState(PinState pinState)
    {
        _pinState = pinState;
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

    public void SetDriverPinStop(bool driverPinStop)
    {
        _driverPinStop = driverPinStop;
    }

    public void SetKeyPinStop(bool keyPinStop)
    {
        _keyPinStop = keyPinStop;
    }
}
