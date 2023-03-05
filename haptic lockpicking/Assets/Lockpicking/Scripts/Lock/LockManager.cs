using UnityEngine;
using static CutoutManager;
using System.Collections.Generic;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance { get; private set; }

    public static LockController Lock { get; private set; }

    [SerializeField]
    LockBuilder _lockBuilder;


    List<int> pinCountParameters = new() { 5, };
    List<List<int>> pinOrderParameters;
    List<bool> respectOrderParameters = new() { true };
    List<bool> hasPickParameters = new() { true };
    List<bool> hasTensionParameters = new() { true };
    List<bool> colorCodePinsParameters = new() { true };
    List<bool> showTensionIndicatorParameters = new() { false };
    List<bool> showPinPositionIndicatorParameters = new() { false };
    List<CutoutState> cutoutStateParameters = new() { CutoutState.FULL };


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception();
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        pinOrderParameters = new() { PinManager.Instance.GetRandomPinOrder(5) };
    }

    public void CreateNewLock(TutorialSectionInformation _information)
    {
        if (Lock != null)
        {
            Destroy(Lock.gameObject);
        }

        Lock = _lockBuilder.BuildLock();

        if (_information.PinOrder == null)
        {
            _information.PinOrder = PinManager.Instance.GetRandomPinOrder(_information.PinCount);
        }

        Debug.Log(_information.PinCount + " " + _information.PinOrder[0] + _information.RespectOrder + _information.HasPick +  _information.HasTension + _information.ColorCodePins + _information.ShowTensionIndicator + _information.ShowPinPositionIndicator + _information.CutoutState);

        _lockBuilder.SetParameters(_information.PinCount, _information.PinOrder, _information.RespectOrder, _information.HasPick, _information.HasTension, _information.ColorCodePins, _information.ShowTensionIndicator, _information.ShowPinPositionIndicator, _information.CutoutState);

        CalibrationManager.Instance.SetIsCalibrated(false);
    }
}
