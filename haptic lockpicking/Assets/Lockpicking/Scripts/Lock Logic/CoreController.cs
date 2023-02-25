using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PinController;
using static TensionForceManager;

public class CoreController : MonoBehaviour
{
    [SerializeField]
    PinManager _pinManager;

    [SerializeField]
    TensionForceManager _tensionForceManager;

    [SerializeField]
    private float _rotateSpeed = 1f;

    [SerializeField]
    GameObject _handVisual;

    [SerializeField]
    GameObject _pick;

    public static bool LockFinished = false;

    public void Update()
    {
        if (_pinManager.UpdatePinLogic() && !LockFinished)
        {
            StartCoroutine(Finish());
            LockFinished = true;

            _handVisual.SetActive(false);
            _pick.SetActive(false);
        }
    }

    private IEnumerator Finish()
    {
        foreach (PinController pins in _pinManager.GetPinControllers())
        {
            pins.GetKeyPin().transform.parent = transform;
            pins.GetKeyPin().GetRigidbody().isKinematic = true;
        }

        
        while(true)
        {
            yield return null;

            if (StaticTensionState == TensionState.LOOSE)
            {
                continue;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 90)), _tensionForceManager.GetFingerPosition01() * _rotateSpeed);

            if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
            {
                break;
            }
        }

        Debug.Log("finished");
       
    }

}
