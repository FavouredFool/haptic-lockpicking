using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportElementManager : MonoBehaviour
{
    [SerializeField]
    GameObject _pickIndicatorCanvas;

    [SerializeField]
    GameObject _forceIndicatorCanvas;

    [SerializeField]
    GameObject _pinColorManager;

    public void Start()
    {
        _pickIndicatorCanvas.SetActive(false);
        _forceIndicatorCanvas.SetActive(false);
        _pinColorManager.SetActive(false);
    }

    public void TogglePinColor(bool active)
    {
        _pinColorManager.SetActive(active);
    }

    public void ToggleForceIndicator(bool active)
    {
        _forceIndicatorCanvas.SetActive(active);
    }

    public void TogglePickIndicator(bool active)
    {
        _pickIndicatorCanvas.SetActive(active);
    }

}
