using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CutoutManager;

public class SupportElementManager : MonoBehaviour
{
    public static SupportElementManager Instance { get; private set; }

    [SerializeField]
    GameObject _forceIndicatorSlider;

    [SerializeField]
    GestureMenuToggle _pinColorToggle;

    [SerializeField]
    GestureMenuToggle _pickIndicatorToggle;

    [SerializeField]
    GestureMenuToggle _forceIndicatorToggle;

    [SerializeField]
    GestureMenuSlider _cutoutSlider;

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

    public void TogglePinColor(bool active)
    {
        PinColorManager.Instance.gameObject.SetActive(active);
    }

    public void ToggleForceIndicator(bool active)
    {
        _forceIndicatorSlider.SetActive(active);
    }

    public void TogglePickIndicator(bool active)
    {
        if (PickManager.Instance.GetPickIndicatorCanvas() != null)
        {
            PickManager.Instance.GetPickIndicatorCanvas().SetActive(active);
        }
        
    }

    public void SetCutout(int cutoutInt)
    {
        CutoutManager.Instance.SetCutoutFromState((CutoutState)cutoutInt);
    }

    public GestureMenuToggle GetPinColorToggle()
    {
        return _pinColorToggle;
    }

    public GestureMenuToggle GetForceIndicatorToggle()
    {
        return _forceIndicatorToggle;
    }

    public GestureMenuToggle GetPickIndicatorToggle()
    {
        return _pickIndicatorToggle;
    }

    public GestureMenuSlider GetCutoutSlider()
    {
        return _cutoutSlider;
    }

}
