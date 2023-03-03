using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PinController;
using static TensionForceManager;

public class CoreController : MonoBehaviour
{
    [SerializeField]
    LockController _lock;

    [SerializeField]
    private float _rotateSpeed = 1f;

    [SerializeField]
    GameObject _pick;

    [SerializeField]
    Transform _tensionTool;

    [SerializeField]
    Transform _touchPoint;

    bool _lockFinished = false;

    public void Update()
    {
        if (AllPinsInOpenPosition() && !_lockFinished)
        {
            _lockFinished = true;

            _pick.SetActive(false);

            StartCoroutine(Finish());
        }
    }

    public bool AllPinsInOpenPosition()
    {
        if (_lock == null)
        {
            return false;
        }

        foreach(PinController pinController in PinManager.Instance.GetPinControllers())
        {
            if (!pinController.GetPinIsInOpenPosition())
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator Finish()
    {
        foreach (PinController pins in PinManager.Instance.GetPinControllers())
        {
            pins.GetKeyPin().transform.parent = transform;
            pins.GetKeyPin().GetRigidbody().isKinematic = true;
        }

        
        while(true)
        {
            yield return null;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 90)), _rotateSpeed);

            if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
            {
                break;
            }
        }

        Debug.Log("finished");
       
    }

    public void ResetLock()
    {
        PinManager.Instance.RandomizePins();
    }

    public Transform GetTensionTool()
    {
        return _tensionTool;
    }

    public bool GetLockFinished()
    {
        return _lockFinished;
    }

    public Transform GetTouchPoint()
    {
        return _touchPoint;
    }
}
