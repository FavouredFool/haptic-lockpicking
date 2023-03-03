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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateNewLock(0);
        }
    }

    public void CreateNewLock(int type)
    {
        if (Lock != null)
        {
            Destroy(Lock.gameObject);
        }

        Lock = _lockBuilder.BuildLock();
        _lockBuilder.SetParameters(pinCountParameters[type], pinOrderParameters[type], respectOrderParameters[type], hasPickParameters[type], hasTensionParameters[type], colorCodePinsParameters[type], showTensionIndicatorParameters[type], showPinPositionIndicatorParameters[type], cutoutStateParameters[type]);

        CalibrationManager.Instance.SetIsCalibrated(false);
    }
}
