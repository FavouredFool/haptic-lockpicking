using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CutoutManager;

public class SupportElementManager : MonoBehaviour
{
    [SerializeField]
    CutoutManager _cutoutManager;

    [SerializeField]
    PickManager _pickManager;

    [SerializeField]
    GameObject _forceIndicatorCanvas;

    [SerializeField]
    GameObject _pinColorManager;

    public void Awake()
    {
        _pickManager.GetPickIndicatorCanvas().SetActive(true);
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
        _pickManager.GetPickIndicatorCanvas().SetActive(active);
    }

    public void SetCutout(int cutoutInt)
    {
        _cutoutManager.SetCutoutFromState((CutoutState)cutoutInt);
    }

}
