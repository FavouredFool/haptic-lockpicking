using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PinController;
using static TensionForceManager;

public class SupportElementManager : MonoBehaviour
{

    [SerializeField]
    PinManager _pinManager;

    [Header("PinColors")]
    [SerializeField]
    Color driverSetColor;

    [SerializeField]
    Color driverBindingColor;

    [SerializeField]
    Color driverLockedColor;

    [SerializeField]
    Color keyTouchedColor;

    [SerializeField]
    Color keyLockedColor;

    public void OnEnable()
    {


        // Update Pick Visual Clues
    }

    public void OnDisable()
    {
        foreach (PinController pinController in _pinManager.GetPinControllers())
        {
            pinController.ResetPinColor();
        }
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
            return keyLockedColor;
        }

        if (pinController.GetKeyPin().IsBeingTouched())
        {
            return keyTouchedColor;
        }

        return pinController.GetKeyPin().GetDefaultColor();
    }

    Color CalculateDriverPinColor(PinController pinController)
    {
        if (pinController.GetPinState() == PinState.SET)
        {
            return driverSetColor;
        }

        if (StaticTensionState == TensionState.LOCKED)
        {
            return driverLockedColor;
        }

        if (pinController.GetPinState() == PinState.BINDING)
        {
            return driverBindingColor;
        }

        return pinController.GetDriverPin().GetDefaultColor();
    }
}
