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
    private float _rotateSpeed = 1f;

    [SerializeField]
    GameObject _handVisual;

    public static bool LockFinished = false;

    public void Update()
    {
        if (_pinManager.UpdatePinLogic() && !LockFinished)
        {
            // Rotate Core open. In the end this is either automatic or sensitive to push.
            StartCoroutine(Finish());
            LockFinished = true;
            _handVisual.SetActive(false);
        }
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 90)), _rotateSpeed);

            if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
            {
                break;
            }
        }

        Debug.Log("finished");
       
    }

}
