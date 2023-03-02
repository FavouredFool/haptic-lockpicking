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
    bool _turnsOnlyWithForce = true;

    public bool _lockFinished = false;

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
        foreach(PinController pinController in _lock.GetPinManager().GetPinControllers())
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
        foreach (PinController pins in _lock.GetPinManager().GetPinControllers())
        {
            pins.GetKeyPin().transform.parent = transform;
            pins.GetKeyPin().GetRigidbody().isKinematic = true;
        }

        
        while(true)
        {
            yield return null;

            if (StaticTensionState == TensionState.LOOSE && _turnsOnlyWithForce)
            {
                continue;
            }

            float speed = _turnsOnlyWithForce ? _lock.GetTensionForceManager().GetFingerPosition01() * _rotateSpeed : _rotateSpeed/3;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 90)), speed);

            if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
            {
                break;
            }
        }

        Debug.Log("finished");
       
    }

    public void ResetLock()
    {
        _lock.GetPinManager().RandomizePins();
    }

    public Transform GetTensionTool()
    {
        return _tensionTool;
    }

    public bool GetLockFinished()
    {
        return _lockFinished;
    }
}
