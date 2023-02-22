using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PinController;

public class CoreController : MonoBehaviour
{
    [SerializeField]
    PinManager _pinManager;

    private bool _lockFinished = false;

    private float _rotateSpeed = 1f;

    public void Update()
    {
        if (_pinManager.UpdatePinLogic() && !_lockFinished)
        {
            // Rotate Core open. In the end this is either automatic or sensitive to push.
            StartCoroutine(Finish());
            _lockFinished = true;
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 90)), _rotateSpeed);

            if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
            {
                break;
            }
        }

        Debug.Log("finished");
       
    }

}
