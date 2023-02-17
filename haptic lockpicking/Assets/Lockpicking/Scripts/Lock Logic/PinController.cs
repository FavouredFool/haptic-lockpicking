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

    private PinState _pinState = PinState.LOOSE;

    public void Awake()
    {
        _driverPinBlockade.enabled = false;
    }



    public void Update()
    {

        SwitchPinState();

        // Check if Lock is openable
        _isOpen = _driverPin.IsBelowSheer() && _pinState == PinState.MOVABLE;

       
        // Screw is always attached
        float offset = _driverPin.transform.position.y - _screw.transform.position.y;
        float diff = offset - CONSTANT_SCREW_OFFSET;
        _screw.transform.localScale = new Vector3(1f, 1 + diff * CONSTANT_SCREW_MULTIPLIER, 1f);
    }

    protected void SwitchPinState()
    {
        _keyPin.AnyStateUpdate(this);
        _driverPin.AnyStateUpdate(this);

        switch (_pinState)
        {
            case PinState.LOOSE:
                _keyPin.LooseUpdate(this);
                _driverPin.LooseUpdate(this);
                break;
            case PinState.MOVABLE:
                _keyPin.MovableUpdate(this);
                _driverPin.MovableUpdate(this);
                break;
            case PinState.LOCKED:
                _keyPin.LockedUpdate(this);
                _driverPin.LockedUpdate(this);
                break;
        }
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

    public void ActivateDriverPinBlockade()
    {
        _driverPinBlockade.enabled = true;
    }
}
