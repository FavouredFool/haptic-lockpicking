using UnityEngine;

public class PinController : MonoBehaviour
{
    [SerializeField]
    private KeyPin _keyPin;

    [SerializeField]
    private DriverPin _driverPin;

    [SerializeField]
    private Transform _screw;

    public static float CONSTANT_DRIVER_OFFSET = 0.53f;

    public static float CONSTANT_SCREW_OFFSET = 1.448f;

    public static float CONSTANT_SCREW_MULTIPLIER = 0.687f;

    public static float SHEERLINE_HEIGHT = -0.99f;

    private bool _isOpen = false;


    public void Update()
    {
        if (_keyPin.GetBelowSheer())
        {
            _driverPin.SetMaxDriverHeight(SHEERLINE_HEIGHT - CONSTANT_DRIVER_OFFSET);
        }

        _driverPin.SetAttachedPosition(_keyPin.transform.position);

        // Check if Lock is openable
        _isOpen = _driverPin.GetMaxDriverHeight() <= SHEERLINE_HEIGHT;

       
        // Screw is always attached
        float offset = _driverPin.transform.position.y - _screw.transform.position.y;
        float diff = offset - CONSTANT_SCREW_OFFSET;
        _screw.transform.localScale = new Vector3(1f, 1 + diff * CONSTANT_SCREW_MULTIPLIER, 1f);
    }

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public KeyPin GetKeyPin()
    {
        return _keyPin;
    }
}
