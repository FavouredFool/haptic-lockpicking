using UnityEngine;

public class PickManager : MonoBehaviour
{
    public static PickManager Instance { get; private set; }

    [SerializeField]
    Transform _pickDriver;

    bool _hasPick = false;

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

    public PickController GetPickController()
    {
        if (LockManager.Lock == null)
        {
            return null;
        }

        return LockManager.Lock.GetPickController();
    }

    public GameObject GetPickIndicatorCanvas()
    {
        if (LockManager.Lock == null)
        {
            return null;
        }

        return LockManager.Lock.GetPickController().GetPickIndicatorCanvas();
    }

    public Transform GetPickDriver()
    {
        return _pickDriver;
    }

    public void SetHasPick(bool hasPick)
    {
        _hasPick = hasPick;
    }

    public bool GetHasPick()
    {
        return _hasPick;
    }
}
