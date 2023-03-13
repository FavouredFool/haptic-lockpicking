using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField]
    List<PinController> _pinControllers;

    [SerializeField]
    PickController _pickController;

    [SerializeField]
    PickSpectre _pickSpectre;

    [SerializeField]
    CoreController _coreController;

    [SerializeField]
    Rigidbody _key;

    [Header("Core")]
    [SerializeField]
    List<GameObject> _coreParts;

    [Header("Hull")]
    [SerializeField]
    List<GameObject> _hullParts;



    public List<PinController> GetPinControllers()
    {
        return _pinControllers;
    }

    public PickController GetPickController()
    {
        return _pickController;
    }

    public PickSpectre GetPickSpectre()
    {
        return _pickSpectre;
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

    public Rigidbody GetKey()
    {
        return _key;
    }
}
