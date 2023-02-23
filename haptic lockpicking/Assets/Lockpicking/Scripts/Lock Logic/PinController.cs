using UnityEngine;
using static TensionForceManager;

public class PinController : MonoBehaviour
{
    public enum PinState { SPRINGY, BINDING, SET }

    [SerializeField]
    private KeyPin _keyPin;

    [SerializeField]
    private DriverPin _driverPin;

    [SerializeField]
    private BoxCollider _driverPinBlockade;

    [SerializeField]
    private Transform _screw;

    [SerializeField]
    private TensionForceManager _tensionForceManager;

    [SerializeField, Range(0, 1)]
    private float _maxVelocityForSet = 0.25f;

    [SerializeField, Range(0, 1)]
    private float _setThreshold = 0.25f;

    public static float CONSTANT_DRIVER_OFFSET = 0.53f;

    public static float CONSTANT_SCREW_OFFSET = 1.448f;

    public static float CONSTANT_SCREW_MULTIPLIER = 0.687f;

    public static float SHEERLINE_HEIGHT = -0.99f;

    private PinState _setState = PinState.SPRINGY;

    private PinState _nonSetState = PinState.SPRINGY;

    public void Awake()
    {
        _driverPinBlockade.enabled = false;
    }

    public void FixedUpdate()
    {
        AnimatePins();

        CalculateSetState();
    }

    public void Update()
    {
        AnimateScrew();
    }

    protected void AnimateScrew()
    {
        float offset = _driverPin.transform.position.y - _screw.transform.position.y;
        float diff = offset - CONSTANT_SCREW_OFFSET;
        _screw.transform.localScale = new Vector3(1f, 1 + diff * CONSTANT_SCREW_MULTIPLIER, 1f);
    }

    protected void AnimatePins()
    {
        switch(StaticTensionState)
        {
            case TensionState.LOOSE:
                AnimatePinActive(_keyPin);
                AnimatePinActive(_driverPin);
                break;
            case TensionState.MOVABLE:
                if (_setState == PinState.SET)
                {
                    AnimatePinStatic(_driverPin);
                    AnimatePinStatic(_keyPin);
                }
                else
                {
                    AnimatePinActive(_keyPin);
                    AnimatePinActive(_driverPin);
                }
                break;
            case TensionState.LOCKED:
                AnimatePinStatic(_keyPin);
                AnimatePinStatic(_driverPin);
                break;
        }
    }

    void AnimatePinActive(Pin pin)
    {
        pin.ActivatePinMovement();
        pin.PhysicsUpdate();
    }

    void AnimatePinStatic(Pin pin)
    {
        pin.FreezePinMovement();
    }

    public void CalculateSetState()
    {
        bool tensionIsNotLoose = StaticTensionState != TensionState.LOOSE;
        bool isOnSheer = _driverPin.IsOnSheer(this);
        bool pinIsSlow = Mathf.Abs(_driverPin.GetVelocity()) <= _maxVelocityForSet;

        PinState previousState = _setState;

        _setState = (tensionIsNotLoose && isOnSheer && pinIsSlow && _nonSetState == PinState.BINDING) ? PinState.SET : _nonSetState;

        DriverPinBlockadeActive(_setState == PinState.SET);

        if (_setState == PinState.SET && _setState != previousState)
        {
            _tensionForceManager.PinHasBeenSet();
        }

        
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

    public PinState GetSetState()
    {
        return _setState;
    }

    public float GetSetThreshold()
    {
        return _setThreshold;
    }

    public void SetNonSetState(PinState nonSetState)
    {
        _nonSetState = nonSetState;
    }
}
