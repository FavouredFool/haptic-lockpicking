using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField]
    List<PinController> _pinControllers;

    [SerializeField]
    PickController _pickController;

    [SerializeField]
    CoreController _coreController;

    [Header("Core")]
    [SerializeField]
    List<GameObject> _coreParts;

    [Header("Hull")]
    [SerializeField]
    List<GameObject> _hullParts;

    PinManager _pinManager;
    TensionForceManager _tensionForceManager;
    PickManager _pickManager;
    Camera _camera;

    public List<PinController> GetPinControllers()
    {
        return _pinControllers;
    }

    public PickController GetPickController()
    {
        return _pickController;
    }

    public CoreController GetCoreController()
    {
        return _coreController;
    }

    public List<GameObject> GetCoreParts()
    {
        return _coreParts;
    }

    public List<GameObject> GetHullParts()
    {
        return _hullParts;
    }

    public PinManager GetPinManager()
    {
        return _pinManager;
    }

    public TensionForceManager GetTensionForceManager()
    {
        return _tensionForceManager;
    }

    public PickManager GetPickManager()
    {
        return _pickManager;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

}
