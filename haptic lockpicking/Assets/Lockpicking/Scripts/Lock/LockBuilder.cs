using UnityEngine;

public class LockBuilder : MonoBehaviour
{
    [SerializeField]
    Transform _lockParent;

    [SerializeField]
    LockController _lockPrefab;

    

    private void Start()
    {
        //BuildLock();
    }

    public LockController BuildLock()
    {
        LockController builtLock = Instantiate(_lockPrefab, _lockParent);

        return builtLock;
    }
}
