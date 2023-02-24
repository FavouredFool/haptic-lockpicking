using UnityEngine;
using static PinController;
using static TensionForceManager;

public class PinColorManager : MonoBehaviour
{
    [SerializeField]
    PinManager _pinManager;

    [Header("PinColors")]
    [SerializeField]
    Color _driverSetColor;

    [SerializeField]
    Color _driverBindingColor;

    [SerializeField]
    Color _driverLockedColor;

    [SerializeField]
    Color _keyTouchedColor;

    [SerializeField]
    Color _keyLockedColor;

    void OnDisable()
    {
        foreach (PinController pinController in _pinManager.GetPinControllers())
        {
            pinController.ResetPinColor();
        }
    }


    public void Update()
    {
        foreach (PinController pinController in _pinManager.GetPinControllers())
        {
            pinController.ColorCodePins(CalculateDriverPinColor(pinController), CalculateKeyPinColor(pinController));
        }
    }

    Color CalculateKeyPinColor(PinController pinController)
    {
        if (StaticTensionState == TensionState.LOCKED)
        {
            return _keyLockedColor;
        }

        if (pinController.GetKeyPin().IsBeingTouched() && StaticTensionState != TensionState.LOCKED)
        {
            return _keyTouchedColor;
        }

        return pinController.GetKeyPin().GetDefaultColor();
    }

    Color CalculateDriverPinColor(PinController pinController)
    {
        if (pinController.GetPinState() == PinState.SET)
        {
            return _driverSetColor;
        }

        if (StaticTensionState == TensionState.LOCKED)
        {
            return _driverLockedColor;
        }

        if (pinController.GetPinState() == PinState.BINDING)
        {
            return _driverBindingColor;
        }

        return pinController.GetDriverPin().GetDefaultColor();
    }
}
