using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PinController;
using static TensionForceManager;

public class SupportElementManager : MonoBehaviour
{

    [SerializeField]
    PinManager _pinManager;

    [SerializeField]
    Canvas _pickIndicatorCanvas;

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


    public void OnEnable()
    {
        _pickIndicatorCanvas.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        foreach (PinController pinController in _pinManager.GetPinControllers())
        {
            pinController.ResetPinColor();
        }

        _pickIndicatorCanvas.gameObject.SetActive(false);
    }

    public void Update()
    {
        // Update Force

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

        if (pinController.GetKeyPin().IsBeingTouched())
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
