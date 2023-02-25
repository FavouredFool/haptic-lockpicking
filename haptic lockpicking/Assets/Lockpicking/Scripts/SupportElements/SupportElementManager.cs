using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CutoutManager;

public class SupportElementManager : MonoBehaviour
{
    [SerializeField]
    CutoutManager _cutoutManager;

    [SerializeField]
    GameObject _pickIndicatorCanvas;

    [SerializeField]
    GameObject _forceIndicatorCanvas;

    [SerializeField]
    GameObject _pinColorManager;

    public void Start()
    {
        _pickIndicatorCanvas.SetActive(true);
        _forceIndicatorCanvas.SetActive(false);
        _pinColorManager.SetActive(true);
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

    public void SetCutout(float value)
    {
        _cutoutManager.SetCutoutFromState((CutoutState)value);
    }

}
