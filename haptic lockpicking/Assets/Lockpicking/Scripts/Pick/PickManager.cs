using UnityEngine;

public class PickManager : MonoBehaviour
{
    public static PickManager Instance { get; private set; }

    [SerializeField]
    LockController _lock;

    [SerializeField]
    Transform _pickDriver;

    public PickController GetPickController()
    {
        if (_lock == null)
        {
            return null;
        }

        return _lock.GetPickController();
    }

    public GameObject GetPickIndicatorCanvas()
    {
        if (_lock == null)
        {
            return null;
        }

        return _lock.GetPickController().GetPickIndicatorCanvas();
    }

    public Transform GetPickDriver()
    {
        return _pickDriver;
    }
}
