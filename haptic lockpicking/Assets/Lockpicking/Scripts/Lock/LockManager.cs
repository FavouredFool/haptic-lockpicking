using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance { get; private set; }

    public static LockController Lock { get; private set; }

    [SerializeField]
    LockBuilder _lockBuilder;

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

    public void CreateNewLock()
    {
        Lock = _lockBuilder.BuildLock();
    }
}
