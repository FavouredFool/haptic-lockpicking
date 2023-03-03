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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateNewLock();
        }
    }

    public void CreateNewLock()
    {
        if (Lock != null)
        {
            Destroy(Lock.gameObject);
        }

        

        Lock = _lockBuilder.BuildLock();

        CalibrationManager.Instance.SetIsCalibrated(false);
    }
}
