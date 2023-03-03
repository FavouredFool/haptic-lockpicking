using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CutoutManager;

public class SupportElementManager : MonoBehaviour
{
    public static SupportElementManager Instance { get; private set; }

    [SerializeField]
    GameObject _forceIndicatorCanvas;

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
        _forceIndicatorCanvas.SetActive(active);
    }

    public void TogglePickIndicator(bool active)
    {
        PickManager.Instance.GetPickIndicatorCanvas().SetActive(active);
    }

    public void SetCutout(int cutoutInt)
    {
        CutoutManager.Instance.SetCutoutFromState((CutoutState)cutoutInt);
    }

}
