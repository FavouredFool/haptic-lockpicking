using UnityEngine;

public class LockBuilder : MonoBehaviour
{
    [SerializeField]
    Transform _lockParent;

    [SerializeField]
    LockController _lockPrefab;

    private void Start()
    {
        BuildLock();
    }

    public LockController BuildLock()
    {
        return Instantiate(_lockPrefab, _lockParent);
    }
}
