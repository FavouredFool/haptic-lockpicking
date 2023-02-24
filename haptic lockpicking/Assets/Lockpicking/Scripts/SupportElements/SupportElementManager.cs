using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportElementManager : MonoBehaviour
{
    [SerializeField]
    Canvas _pickIndicatorCanvas;

    [SerializeField]
    Canvas _forceIndicatorCanvas;

    [SerializeField]
    PinColorManager _pinColorManager;

    


    public void OnEnable()
    {
        _pickIndicatorCanvas.gameObject.SetActive(true);
        _forceIndicatorCanvas.gameObject.SetActive(true);
        _pinColorManager.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        _pickIndicatorCanvas.gameObject.SetActive(false);
        _forceIndicatorCanvas.gameObject.SetActive(false);
        _pinColorManager.gameObject.SetActive(false);
    }

}
