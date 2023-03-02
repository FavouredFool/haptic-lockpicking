using UnityEngine;

public class PickManager : MonoBehaviour
{
    [SerializeField]
    LockController _lock;

    [SerializeField]
    Transform _pickDriver;

    public PickController GetPickController()
    {
        return _lock.GetPickController();
    }

    public GameObject GetPickIndicatorCanvas()
    {
        return _lock.GetPickController().GetPickIndicatorCanvas();
    }

    public Transform GetPickDriver()
    {
        return _pickDriver;
    }
}
