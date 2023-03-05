using UnityEngine;
using System.Collections.Generic;
using static CutoutManager;

public class LockBuilder : MonoBehaviour
{
    [SerializeField]
    Transform _lockParent;

    [SerializeField]
    LockController _lockPrefab;


    public LockController BuildLock()
    {
        LockController builtLock = Instantiate(_lockPrefab, _lockParent);

        // Parametrize Lock-Creation

        // Pin-Fundamentals
        // 1. how many pins should the lock have? 1-5
        // 2. In which order should the pins be? (put in random order if random)
        // 3. Should order be respected or should all pins be settable?

        // Tools
        // 4. Should lock have a pick?
        // 5. Should lock have a tension-tool and Tension?

        // Help
        // 6. Should the pins be color-coded?
        // 7. Should tension-indicator be displayed?
        // 8. Should Pick have PinPosition Indicator?
        // 9. Which cutout should be used? NONE, PARTIAL, FULL

        return builtLock;
    }

    public void SetParameters(int pinAmount, List<int> pinOrder, bool respectOrder, bool hasPick, bool hasTension, bool colorCodePins, bool showTensionIndicator, bool showPinPositionIndicator, CutoutState cutoutState)
    {
        SetPinFundamentals(pinAmount, pinOrder, respectOrder);
        SetTools(hasPick, hasTension);
        SetHelp(colorCodePins, showTensionIndicator, showPinPositionIndicator, cutoutState);
    }

    void SetPinFundamentals(int pinAmount, List<int> pinOrder, bool respectOrder)
    {
        if (pinAmount < 1 || pinAmount > 5)
        {
            throw new System.Exception();
        }

        if (pinOrder.Count != pinAmount)
        {
            throw new System.Exception();
        }

        PinManager.Instance.SetPinAmount(pinAmount);

        PinManager.Instance.SetPinOrder(pinOrder);

        Debug.Log("respect order: " + respectOrder);

    }

    void SetTools(bool hasPick, bool hasTension)
    {
        Debug.Log("has Pick: " + hasPick);
        Debug.Log("has tension: " + hasTension);
    }

    void SetHelp(bool colorCodePins, bool showTensionIndicator, bool showPinPositionIndicator, CutoutState cutoutState)
    {
        SupportElementManager.Instance.TogglePinColor(colorCodePins);
        SupportElementManager.Instance.ToggleForceIndicator(showTensionIndicator);
        SupportElementManager.Instance.TogglePickIndicator(showPinPositionIndicator);
        SupportElementManager.Instance.SetCutout((int)cutoutState);
    }

}
